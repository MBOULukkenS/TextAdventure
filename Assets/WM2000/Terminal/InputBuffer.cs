using System.IO;

namespace WM2000.Terminal
{
    public class InputBuffer : TextReader
    {
        private string _currentInputLine = string.Empty;

        public delegate void CommandSentHandler(string command);
        public event CommandSentHandler CommandSent;

        public bool InputEnabled { get; set; } = true;

        public void ReceiveFrameInput(string input)
        {
            if (!InputEnabled)
                return;
            
            foreach (char c in input)
                UpdateCurrentInputLine(c);
        }

        public string GetCurrentInputLine()
        {
            return _currentInputLine;
            // unless password
        }

        public override string ToString()
        {
            return GetCurrentInputLine();
        }

        private void UpdateCurrentInputLine(char c)
        {
            switch (c)
            {
                case '\b':
                    DeleteCharacters();
                    break;
                case '\r':
                case '\n':
                    SendCommand(_currentInputLine);
                    break;
                default:
                    _currentInputLine += c;
                    break;
            }
        }

        private void DeleteCharacters(int count = 1)
        {
            if (_currentInputLine.Length == 0)
                return;
            
            for (int i = 0; i < (count > _currentInputLine.Length ? _currentInputLine.Length : count); i++)
                _currentInputLine = _currentInputLine.Remove(_currentInputLine.Length - 1);
        }

        private void SendCommand(string command)
        {
            CommandSent?.Invoke(command);
            _currentInputLine = string.Empty;
        }
    }
}
