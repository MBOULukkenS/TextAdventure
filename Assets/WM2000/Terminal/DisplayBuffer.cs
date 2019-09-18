using System.Collections.Generic;
using System.Linq;

namespace WM2000.Terminal
{
    public class DisplayBuffer
    {
        private List<string> _logLines = new List<string>();

        private InputBuffer _inputBuffer;
        private const float FlashInterval = .5f;

        public DisplayBuffer(InputBuffer inputBuffer)
        {
            _inputBuffer = inputBuffer;
            inputBuffer.OnCommandSent += OnCommand;
        }

        public void WriteLine(string line)
        {
            _logLines.Add(line);
        }

        public void Clear()
        {
            _logLines = new List<string>();
        }

        public string GetDisplayBuffer(float time, int width, int height)
        {
            string lines = GetAllDisplayLines(time);
            string wrappedLines = Wrap(width, lines);
            string viewportLines = CutViewport(height, wrappedLines);
            return viewportLines;
        }

        private string GetAllDisplayLines(float time)
        {
            string output = _logLines.Aggregate("", (current, line) => current + (line + '\n'));

            output += _inputBuffer.GetCurrentInputLine();
            output += GetFlashingCursor(time);
            return output;
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
                {
                    return output;
                }
                if (lines[i] == '\n')
                {
                    rowCount++;
                }
                output = lines[i] + output;
            }
            return output;
        }

        private char GetFlashingCursor(float time)
        {
            if (time % (2 * FlashInterval) <= FlashInterval)
            {
                return '_';
            }
            else
            {
                return ' ';
            }
        }

        private void OnCommand(string command)
        {
            _logLines.Add(command);
        }
    }
}