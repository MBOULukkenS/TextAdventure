using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.SynonymDict;
using UnityEngine;
using Utility;
using WM2000.Terminal;
using Yarn;
using Yarn.Unity;

namespace TerminalUI
{
    public class Wm2000DialogueUi : DialogueUIBehaviour
    {
        private OptionChooser _setCurrentOption;
        private Dictionary<int, string[]> _currentOptions = new Dictionary<int, string[]>();

        private SynonymDict _synonymDict;

        private IEnumerable<Commands.Command> _commands;

        [Header("Settings")]
        public bool DisplayOptions = true;
        public bool AllowNumberInput = true;
        public string OptionsText = "Opties";
        public string OptionText = "Optie";
        public int LineInterval = 250;

        [Header("Strings")]
        public string DialogueFinishedMessage = "Spel afgelopen. Druk op ESC om over nieuw te beginnen.";
        public string InvalidChoiceText = "I dont know how to [command]";
        
        public static Wm2000DialogueUi Instance { get; private set; }

        private void Start()
        {
            _commands = ReflectionHelper.GetDerivingInstances<Commands.Command>();
            _synonymDict = GetComponent<SynonymDict>();
            
            Instance = this;
            
            Terminal.PrimaryTerminal.CommandSent += OnInputReceived;
        }

        public override IEnumerator RunLine(Line line)
        {
            Terminal.PrimaryInputActive = false;

            yield return Terminal.TypeLine(line.text);
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
                _currentOptions.Add(i, Utilities.SplitArgumentString(optionString));
                i++;
            }
            
            Terminal.PrintSeparator();
            
            if (DisplayOptions)
            {
                yield return Terminal.TypeLine($"{OptionsText}:");

                i = 1;
                foreach (KeyValuePair<int, string[]> option in _currentOptions)
                {
                    yield return Terminal.TypeLine(
                        $"{OptionText} {i}: '{option.Value.Combine()}'");
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
            yield return Terminal.TypeLine(DialogueFinishedMessage);
            
            while (Input.GetKeyDown(KeyCode.Escape) == false)
                yield return null;

            Terminal.ClearScreen();
            GameObject.Find("WM2000")
                .GetComponent<DialogueRunner>()
                .StartDialogue();
        }

        public override IEnumerator RunCommand(Command command)
        {
            string[] args = command.text.Split(Globals.Separator);
            Commands.Command commandToExecute;

            try
            {
                commandToExecute = _commands
                    .First(c => c
                        .GetType().Name
                        .ToLower()
                        .Equals($"{args[0].ToLower()}command", StringComparison.CurrentCultureIgnoreCase)
                    );
            }
            catch (Exception)
            {
                Debug.LogWarning($"Command: '{args[0]}' not found!");
                yield break;
            }
            
            yield return commandToExecute.Run(args
                .Skip(1)
                .ToArray());
        }

        private void OnInputReceived(string[] args)
        {
            if (_setCurrentOption == null)
                return;

            if (!AllowNumberInput || !int.TryParse(args[0], out int choiceInt))
                choiceInt = -1;

            args = args
                .Select(s => s.ToLower())
                .ToArray();

            try
            {
                KeyValuePair<int, string[]> result = _currentOptions
                    .First(option => option.Key == choiceInt || args
                                    .Where((arg, i) => _synonymDict.IsSynonymFor(arg, option.Value[i]))
                                    .Count() == option.Value.Length);

                ChooseOption(result.Key - 1);
            }
            catch (Exception)
            {
                if (!string.IsNullOrEmpty(InvalidChoiceText))
                    Terminal.WriteLine($"{InvalidChoiceText.Replace("[command]", args.Combine())}");
            }
        }

        private void ChooseOption(int option)
        {
            _setCurrentOption?.Invoke(option);
            
            _currentOptions.Clear();
            _setCurrentOption = null;
        }
    }
}