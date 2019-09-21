using System;
using System.IO;
using System.Linq;
using System.Reflection;
using DefaultNamespace;
using UnityEngine;

namespace WM2000.Terminal
{
    public class Terminal : MonoBehaviour
    {
        public delegate void ParameterizedCommandSentHandler(string[] args);
        public ParameterizedCommandSentHandler CommandSent;
        
        public DisplayBuffer Out { get; private set; }
        public InputBuffer In { get; private set; }

        public bool InputActive
        {
            get => In.InputEnabled;
            set
            {
                In.InputEnabled = value;
                Out.InputEnabled = value;
            }
        }

        public static bool PrimaryInputActive
        {
            get => _primaryTerminal.InputActive;
            set => _primaryTerminal.InputActive = value;
        }

        private int _height;
        private int _width;

        private static Terminal _primaryTerminal;
        public static Terminal PrimaryTerminal => _primaryTerminal;

        private void Awake()
        {
            if (_primaryTerminal == null)  
                _primaryTerminal = this;  // Be the one
            
            In = new InputBuffer();
            Out = new DisplayBuffer(In);

            In.CommandSent += OnCommandSent;
        }

        public string GetDisplayBuffer(int width, int height)
        {
            _height = height;
            _width = width;
            
            return Out.GetDisplayBuffer(width, height);
        }

        public void ReceiveFrameInput(string input)
        {
            In.ReceiveFrameInput(input);
        }

        public static void ClearScreen()
        {
            _primaryTerminal.Out.Clear();
        }

        public static void WriteLine(string line)
        {
            _primaryTerminal.Out.WriteLine(line);
        }

        public static void WriteLine()
        {
            _primaryTerminal.Out.WriteLine(string.Empty);
        }

        public static void Separator(char separatorChar = '-')
        {
            WriteLine(new string(separatorChar, _primaryTerminal._width - 2));
        }

        public static void Write(string line)
        {
            _primaryTerminal.Out.Write(line);
        }

        public static void SetCaret(char active, char inactive)
        {
            _primaryTerminal.Out.CursorChar1 = active;
            _primaryTerminal.Out.CursorChar2 = inactive;
        }

        public static int InputBufferCharCount => _primaryTerminal.In.GetCurrentInputLine().Length;

        private void OnCommandSent(string command)
        {
            CommandSent?.Invoke(command.Split(Globals.Separator));
        }
    }
}