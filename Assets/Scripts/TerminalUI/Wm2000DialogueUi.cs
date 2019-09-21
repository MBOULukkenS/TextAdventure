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

        public int TypingInterval = 100;
        public int LineInterval = 250;

        public float TypingIntervalJitter = 0.2f;
        
        public string DialogueFinishedMessage = "Spel afgelopen. Druk op ESC om over nieuw te beginnen.";

        private void Start()
        {
            Terminal.PrimaryTerminal.CommandSent += OnCommandSent;
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
            
            Terminal.Separator();

            yield return TypeLine("Opties:");
            
            int i = 1;
            foreach (string optionString in optionsCollection.options)
            {
                _currentOptions.Add(i, optionString
                    .ToLower()
                    .Split(Globals.Separator));

                yield return TypeLine($"Optie {i}: '{optionString}'");
                yield return new WaitForSeconds(LineInterval / 1000f);
                i++;
            }
            
            Terminal.Separator();

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
            Debug.Log($"Command: '{command.text}'");

            yield break;
        }

        private void OnCommandSent(string[] args)
        {
            if (_setCurrentOption == null)
                return;
            
            int.TryParse(args[0], out int choiceInt);
            args = args.Select(s => s.ToLower()).ToArray();

            try
            {
                KeyValuePair<int, string[]> result =
                    _currentOptions.First(kvp => kvp.Key == choiceInt || kvp.Value.IsEqual(args));
                
                ChooseOption(result.Key - 1);
            }
            catch (Exception e)
            {
                Terminal.WriteLine($"Invalid choice '{args[0]}'!");
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