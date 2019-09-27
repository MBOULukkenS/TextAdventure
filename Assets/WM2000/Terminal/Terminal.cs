using System;
using System.Collections;
using UnityEngine;
using Utility;

namespace WM2000.Terminal
{
    public class Terminal : MonoBehaviour
    {
        public delegate void ParameterizedCommandSentHandler(string[] args);
        public ParameterizedCommandSentHandler CommandSent;

        [Header("Typing settings")]
        public int TypingInterval = 100;
        public float TypingIntervalJitter = 0.2f;
        
        public DisplayBuffer Out { get; private set; }
        public InputBuffer In { get; private set; }

        /// <summary>
        /// Deze property bepaalt of dat de gebruiker input kan geven in deze Terminal.
        /// </summary>
        public bool InputActive
        {
            get => In.InputEnabled;
            set
            {
                In.InputEnabled = value;
                Out.InputEnabled = value;
            }
        }

        /// <summary>
        /// Deze property bepaalt of dat de gebruiker input kan geven in de primaire Terminal
        /// </summary>
        public static bool PrimaryInputActive
        {
            get => _primaryTerminal.InputActive;
            set => _primaryTerminal.InputActive = value;
        }

        /// <summary>
        /// Deze property stelt de typsnelheid van de primaire Terminal in.
        /// </summary>
        public static int PrimaryTypingInterval
        {
            get => _primaryTerminal.TypingInterval;
            set => _primaryTerminal.TypingInterval = value;
        }

        public History History { get; private set; } = new History();

        private int _height;
        private int _width;

        private bool _initialized = false;

        private static Terminal _primaryTerminal;
        public static Terminal PrimaryTerminal => _primaryTerminal;
        
        private void Awake()
        {
            if (_primaryTerminal == null)  
                _primaryTerminal = this;  // Be the one

            In = new InputBuffer();
            
            In.CommandSent += OnCommandSent;
            In.SpecialKeyPressed += OnSpecialKeySent;
        }

        /// <summary>
        /// Deze functie initialiseert de terminal.
        /// </summary>
        /// <param name="width">Breedte van de Terminal</param>
        /// <param name="height">Hoogte van de Terminal</param>
        public void Initialize(int width, int height)
        {
            _height = height;
            _width = width;
            
            Out = new DisplayBuffer(In, _width, _height);

            _initialized = true;
        }

        /// <summary>
        /// Deze functie maakt een buffer die kan worden weergegeven in een Unity Tekstvak
        /// </summary>
        /// <returns>Tekst die weergegeven kan worden in een Unity Tekstvak</returns>
        /// <exception cref="InvalidOperationException">Deze exception wordt gethrown als de Terminal nog niet geinitialiseert is.</exception>
        public string GetDisplayBuffer()
        {
            if (!_initialized)
                throw new InvalidOperationException("Initialize needs to be called first!");
            
            return Out.GetDisplayBuffer();
        }

        /// <summary>
        /// Deze functie geeft input door aan de InputBuffer.
        /// </summary>
        /// <param name="input">de gebruikersinput</param>
        public void ReceiveFrameInput(string input)
        {
            In.ReceiveFrameInput(input);
            History.ResetIndex();
        }

        /// <summary>
        /// Deze functie geeft een speciaal karakter door aan de InputBuffer.
        /// </summary>
        /// <param name="key">de toets die ingedrukt is</param>
        public void ReceiveSpecialKeyInput(KeyCode key)
        {
            In.ReceiveSpecialKeyInput(key);
        }

        /// <summary>
        /// Deze functie verwijdert alle lines uit de terminal.
        /// </summary>
        public static void ClearScreen()
        {
            _primaryTerminal.Out.Clear();
        }

        /// <summary>
        /// Deze functie schrijft een bericht naar de Terminal en voegt daarna een newline toe.
        /// </summary>
        /// <param name="line">de tekst die moet worden geschreven</param>
        public static void WriteLine(string line)
        {
            _primaryTerminal.Out.WriteLine(line);
        }

        /// <summary>
        /// Voeg een newline toe aan de terminal.
        /// </summary>
        public static void WriteLine()
        {
            _primaryTerminal.Out.WriteLine(string.Empty);
        }

        /// <summary>
        /// Voegt een 'separator' toe in de Terminal.
        /// </summary>
        /// <param name="separatorChar">optioneel: karakter dat wordt gebruikt als 'separator'</param>
        public static void PrintSeparator(char separatorChar = '-')
        {
            WriteLine(new string(separatorChar, (_primaryTerminal._width * 3) - 2));
        }

        /// <summary>
        /// Deze functie schrijft een bericht naar de Terminal zonder een newline toe te voegen.
        /// </summary>
        /// <param name="line">het bericht dat geschreven moet worden</param>
        public static void Write(string line)
        {
            _primaryTerminal.Out.Write(line);
        }

        /// <summary>
        /// Deze functie veranderd de Caret karakter.
        /// </summary>
        /// <param name="active">Caret karakter 1</param>
        /// <param name="inactive">Caret karakter 2</param>
        public static void SetCaret(char active, char inactive)
        {
            _primaryTerminal.Out.CursorChar1 = active;
            _primaryTerminal.Out.CursorChar2 = inactive;
        }
        
        /// <summary>
        /// Typ text naar de terminal in plaats van het er direct naartoe te printen, en voeg daarna een newline toe.
        /// </summary>
        /// <param name="text">de text die moet worden getypt</param>
        /// <returns>Unity Enumerator</returns>
        public static IEnumerator TypeLine(string text)
        {
            yield return TypeLine(text, _primaryTerminal.TypingInterval);
        }

        /// <summary>
        /// Typ text naar de terminal in plaats van het er direct naartoe te printen, en voeg daarna een newline toe.
        /// </summary>
        /// <param name="text">de text die moet worden getypt</param>
        /// <param name="interval">De tijd in milliseconden tussen iedere 'toetsaanslag'</param>
        /// <returns>Unity Enumerator</returns>
        public static IEnumerator TypeLine(string text, int interval)
        {
            yield return Type(text, interval);
            WriteLine();
        }

        /// <summary>
        /// Typ text naar de terminal in plaats van het er direct naartoe te printen
        /// </summary>
        /// <param name="text">de text die moet worden getypt</param>
        /// <returns>Unity Enumerator</returns>
        public static IEnumerator Type(string text)
        {
            yield return Type(text, _primaryTerminal.TypingInterval);
        }
        
        /// <summary>
        /// Typ text naar de terminal in plaats van het er direct naartoe te printen
        /// </summary>
        /// <param name="text"></param>
        /// <param name="interval">De tijd in milliseconden tussen iedere 'toetsaanslag'</param>
        /// <returns>Unity Enumerator</returns>
        public static IEnumerator Type(string text, int interval)
        {
            bool inputEnabled = PrimaryInputActive;
            PrimaryInputActive = false;
            
            foreach (char character in text)
            {
                Write(character.ToString());
                yield return new WaitForSeconds(
                    UnityEngine.Random.Range(
                        _primaryTerminal.TypingInterval 
                            - (interval * _primaryTerminal.TypingIntervalJitter),
                        _primaryTerminal.TypingInterval 
                            + (interval * _primaryTerminal.TypingIntervalJitter)) / 1000f);
            }

            PrimaryInputActive = inputEnabled;
        }

        public static int InputBufferCharCount => _primaryTerminal.In.GetCurrentInputLine().Length;

        /// <summary>
        /// Deze functie wordt uitgevoerd als er input wordt gegeven aan de Terminal.
        /// </summary>
        /// <param name="command">de input die is gegeven</param>
        private void OnCommandSent(string command)
        {
            string[] commandSanitized = Utilities.SplitArgumentString(command);
            
            CommandSent?.Invoke(commandSanitized);
            
            if (commandSanitized.Length > 0)
                History.Add(commandSanitized);
        }

        /// <summary>
        /// Deze functie wordt uitgevoerd als er een speciale toets is ingedrukt, zoals bijvoorbeeld escape.
        /// </summary>
        /// <param name="key">de toets die is ingedrukt</param>
        private void OnSpecialKeySent(KeyCode key)
        {
            if (!InputActive)
                return;
            
            switch (key)
            {
                case Globals.HistoryPreviousKey:
                    In.CurrentInputLine = History
                        .GetPreviousItem()
                        .Combine();
                    break;
                case Globals.HistoryNextKey:
                    In.CurrentInputLine = History
                        .GetNextItem()
                        .Combine();
                    break;
            }
        }
    }
}