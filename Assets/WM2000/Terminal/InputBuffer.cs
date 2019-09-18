namespace WM2000.Terminal
{
    public class InputBuffer
    {
        private string _currentInputLine;

        public delegate void OnCommandSentHandler(string command);
        public event OnCommandSentHandler OnCommandSent;

        public void ReceiveFrameInput(string input)
        {
            foreach (char c in input) 
                UpdateCurrentInputLine(c);
        }

        public string GetCurrentInputLine()
        {
            return _currentInputLine;
        }

        private void UpdateCurrentInputLine(char c)
        {
            if (c == '\b')
                DeleteCharacters();
            else if (c == '\n' || c == '\r')
                SendCommand(_currentInputLine);
            else
                _currentInputLine += c;
        }

        private void DeleteCharacters()
        {
            if (_currentInputLine.Length <= 0) 
                return;
            
            _currentInputLine = _currentInputLine.Remove(_currentInputLine.Length - 1);
        }

        private void SendCommand(string command)
        {
            OnCommandSent?.Invoke(command);
            _currentInputLine = "";
        } 
    }
}
