using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WM2000.Terminal
{
    public class DisplayBuffer : TextWriter
    {
        public const char NewLineChar = '\n';

        private bool _isDirty;
        private string _cachedDisplayBuffer = string.Empty;

        private bool _inputEnabled = true;
        public bool InputEnabled
        {
            get => _inputEnabled;
            set
            {
                _inputEnabled = value;
                _isDirty = true;
            }
        }

        private string AllLines
        {
            get
            {
                if (_isDirty)
                {
                    string output = _logLines.Aggregate("", (current, line) => current + line);

                    _cachedDisplayBuffer = output;
                    _isDirty = false;
                }

                return $"{_cachedDisplayBuffer}{Prompt}{_inputBuffer}{Cursor}";
            }
        }

        private char Cursor => Time.time % (2 * FlashInterval) <= FlashInterval ? CursorChar1 : CursorChar2;
        
        private string _prompt = "> ";
        private string Prompt => InputEnabled ? _prompt : string.Empty;

        public char CursorChar1 { get; set; } = '█';
        public char CursorChar2 { get; set; } = ' ';
    
        List<string> _logLines = new List<string>();

        private readonly InputBuffer _inputBuffer;
        private const float FlashInterval = .5f;
        
        public override Encoding Encoding => Encoding.Default;

        public DisplayBuffer(InputBuffer inputBuffer)
        {
            _inputBuffer = inputBuffer;
            inputBuffer.CommandSent += OnCommand;
        }

        public override void WriteLine(string text)
        {
            _logLines.Add(text + NewLineChar);
            _isDirty = true;
        }

        public override void Write(string text)
        {
            if (_logLines.Count == 0)
                _logLines.Add(text);
            else
                _logLines[_logLines.Count - 1] += text;
            
            _isDirty = true;
        }

        public void Clear()
        {
            _logLines = new List<string>();
        }

        public string GetDisplayBuffer(int width, int height)
        {
            string wrappedLines = Wrap(width, AllLines);

            return CutViewport(height, wrappedLines);
        }

        private string Wrap(int width, string str)
        {
            string output = "";
            int column = 1;
            foreach (char c in str)
            {
                if (column == width)
                {
                    output += '\n';
                    output += c;
                    column = 1;
                }
                else if (c == '\n')
                {
                    output += '\n';
                    column = 1;
                }
                else
                {
                    output += c;
                    column++;
                }
            }
            return output;
        }

        private string CutViewport(int height, string lines)
        {
            string output = "";
            int rowCount = 1;
            for (int i = lines.Length - 1; i >= 0; i--)
            {
                if (rowCount > height)
                    break;
                
                if (lines[i] == '\n') 
                    rowCount++;
                
                output = lines[i] + output;
            }
            return output;
        }
        
        void OnCommand(string command)
        {
            WriteLine(command);
        }
    }
}