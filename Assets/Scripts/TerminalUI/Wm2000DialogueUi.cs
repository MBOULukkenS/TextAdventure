using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DefaultNamespace;
using UnityEngine;
using Utility;
using WM2000.Terminal;
using Yarn;
using Yarn.Unity;
using Random = System.Random;

namespace TerminalUI
{
    public class Wm2000DialogueUi : DialogueUIBehaviour
    {
        private OptionChooser _setCurrentOption;
        private Dictionary<int, string[]> _currentOptions = new Dictionary<int, string[]>();

        private IEnumerable<Commands.Command> _commands;

        [Header("Typing settings")]
        public int TypingInterval = 100;
        public int LineInterval = 250;
        public float TypingIntervalJitter = 0.2f;

        [Header("Option Settings")]
        public bool DisplayOptions = true;
        public bool AllowNumberInput = true;
        public string OptionsText = "Opties";
        public string OptionText = "Optie";

        [Header("Strings")]
        public string DialogueFinishedMessage = "Spel afgelopen. Druk op ESC om over nieuw te beginnen.";
        public string InvalidChoiceText = "I dont know how to [command]";
        
        public static Wm2000DialogueUi Instance { get; private set; }

        private void Start()
        {
            _commands = ReflectionHelper.GetDerivingInstances<Commands.Command>();
            Instance = this;
            
            Terminal.PrimaryTerminal.CommandSent += OnInputReceived;
        }

        public override IEnumerator RunLine(Line line)
        {
            Terminal.PrimaryInputActive = false;

            yield return TypeLine(line.text);
            yield return new WaitForSeconds(LineInterval / 1000f);

            Terminal.PrimaryInputActive = true;
        }

        public override IEnumerator RunOptions(Options optionsCollection, OptionChooser optionChooser)
        {
            _setCurrentOption = optionChooser;
            Terminal.PrimaryInputActive = false;
            
            int i = 1;
            foreach (string optionString in optionsCollection.options)
            {
                _currentOptions.Add(i, optionString
                    .Split(Globals.Separator));
                i++;
            }
            
            Terminal.PrintSeparator();
            
            if (DisplayOptions)
            {
                yield return TypeLine($"{OptionsText}:");

                i = 1;
                foreach (KeyValuePair<int, string[]> option in _currentOptions)
                {
                    yield return TypeLine(
                        $"{OptionText} {i}: '{string.Join(Globals.Separator.ToActualString(), option.Value)}'");
                    yield return new WaitForSeconds(LineInterval / 1000f);
                    i++;
                }

                Terminal.PrintSeparator();
            }

            Terminal.PrimaryInputActive = true;

            while (_setCurrentOption != null)
                yield return null;
        }

        public override IEnumerator DialogueComplete()
        {
            Terminal.PrimaryInputActive = false;
            
            Terminal.WriteLine();
            yield return TypeLine(DialogueFinishedMessage);
            
            while (Input.GetKeyDown(KeyCode.Escape) == false) {
                yield return null;
            }
            
            Terminal.ClearScreen();
            GameObject.Find("WM2000")
                .GetComponent<DialogueRunner>()
                .StartDialogue();
        }

        public override IEnumerator RunCommand(Command command)
        {
            string[] args = command.text.Split(Globals.Separator);

            try
            {
                Commands.Command commandToExecute = _commands
                    .First(c => c
                        .GetType().Name
                        .ToLower()
                        .Equals($"{args[0].ToLower()}command", StringComparison.CurrentCultureIgnoreCase)
                    );
                
                commandToExecute.Run(args
                    .Skip(1)
                    .ToArray());
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Command: '{args[0]}' not found!");
                yield break;
            }

            yield break;
        }

        private void OnInputReceived(string[] args)
        {
            if (_setCurrentOption == null)
                return;

            if (!AllowNumberInput || !int.TryParse(args[0], out int choiceInt))
                choiceInt = -1;

            args = args.Select(s => s.ToLower()).ToArray();

            try
            {
                KeyValuePair<int, string[]> result =
                    _currentOptions.First(kvp => kvp.Key == choiceInt || kvp.Value
                                                     .Select(s => s.ToLower())
                                                     .IsEqual(args));
                
                ChooseOption(result.Key - 1);
            }
            catch (Exception e)
            {
                if (!string.IsNullOrEmpty(InvalidChoiceText))
                    Terminal.WriteLine($"{InvalidChoiceText.Replace("[command]", string.Join(Globals.Separator.ToActualString(), args))}");
            }
        }

        public void ChooseOption(int option)
        {
            _setCurrentOption?.Invoke(option);
            
            _currentOptions.Clear();
            _setCurrentOption = null;
        }

        private IEnumerator TypeLine(string text)
        {
            yield return Type(text);
            Terminal.WriteLine();
        }

        private IEnumerator Type(string text)
        {
            bool inputEnabled = Terminal.PrimaryInputActive;
            Terminal.PrimaryInputActive = false;
            
            foreach (char character in text)
            {
                Terminal.Write(character.ToString());
                yield return new WaitForSeconds(
                    UnityEngine.Random.Range(
                        TypingInterval - (TypingInterval * TypingIntervalJitter),
                        TypingInterval + (TypingInterval * TypingIntervalJitter)) / 1000f);
            }

            Terminal.PrimaryInputActive = inputEnabled;
        }
    }
}