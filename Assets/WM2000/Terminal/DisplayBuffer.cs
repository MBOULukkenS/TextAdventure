using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WM2000.Terminal
{
    /// <summary>
    /// Dit object regelt de text output naar de het WM2000 terminal scherm.
    /// </summary>
    public class DisplayBuffer : TextWriter
    {
        public const char NewLineChar = '\n';

        private int _height;
        private int _width;

        private bool _isDirty;
        private string _cachedDisplayBuffer = string.Empty;

        /// <summary>
        /// Deze property bepaalt of dat de gebruiker input kan leveren of niet.
        /// </summary>
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

        /// <summary>
        /// Deze property combineert alle loglines, en zorgt ervoor dat loglines niet te lang wordt.
        /// </summary>
        private string AllLines
        {
            get
            {
                if (_isDirty)
                {
                    //lines buiten de viewport worden automatisch verwijdert uit de loglines lijst.
                    if (_logLines.Count > _height)
                        _logLines.RemoveRange(0, _logLines.Count - _height);
                    
                    //Combineer alle loglines
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

        /// <summary>
        /// Constructor voor de DisplayBuffer
        /// </summary>
        /// <param name="inputBuffer">De inputbuffer die gebruikt wordt</param>
        /// <param name="width">De breedte van de terminal</param>
        /// <param name="height">De hoogte van de terminal</param>
        public DisplayBuffer(InputBuffer inputBuffer, int width, int height)
        {
            _inputBuffer = inputBuffer;
            _width = width;
            _height = height;
            
            inputBuffer.CommandSent += OnCommand;
        }

        /// <summary>
        /// Schrijft een text naar de terminal, en voegt ook een newline toe.
        /// </summary>
        /// <param name="text">De text die moet worden geschreven</param>
        public override void WriteLine(string text)
        {
            _logLines.Add(text + NewLineChar);
            _isDirty = true;
        }

        /// <summary>
        /// Schrijft een text naar de terminal zonder een newline toe te voegen.
        /// </summary>
        /// <param name="text">De text die moet worden geschreven</param>
        public override void Write(string text)
        {
            if (_logLines.Count == 0)
                _logLines.Add(text);
            else
                _logLines[_logLines.Count - 1] += text;
            
            _isDirty = true;
        }

        /// <summary>
        /// Verwijdert alle entries uit loglines
        /// </summary>
        public void Clear()
        {
            _logLines = new List<string>();
        }

        public string GetDisplayBuffer()
        {
            //string wrappedLines = Wrap(width, AllLines);
            return CutViewport(_height, AllLines);
        }

        private string Wrap(int width, string str)
        {
            string output = string.Empty;
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