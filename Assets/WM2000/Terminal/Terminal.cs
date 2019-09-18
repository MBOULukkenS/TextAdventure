using System.Reflection;
using UnityEngine;

namespace WM2000.Terminal
{
    public class Terminal : MonoBehaviour
    {
        private DisplayBuffer _displayBuffer;
        private InputBuffer _inputBuffer;

        private static Terminal _primaryTerminal;

        private void Awake()
        {
            if (_primaryTerminal == null) { _primaryTerminal = this; } // Be the one
            _inputBuffer = new InputBuffer();
            _displayBuffer = new DisplayBuffer(_inputBuffer);
            _inputBuffer.OnCommandSent += NotifyCommandHandlers;
        }

        public string GetDisplayBuffer(int width, int height)
        {
            return _displayBuffer.GetDisplayBuffer(Time.time, width, height);
        }

        public void ReceiveFrameInput(string input)
        {
            _inputBuffer.ReceiveFrameInput(input);
        }

        public static void Clear()
        {
            _primaryTerminal._displayBuffer.Clear();
        }

        public static void Write(string text)
        {
            _primaryTerminal._inputBuffer.ReceiveFrameInput(text);
        }

        public static void WriteLine(string line)
        {
            _primaryTerminal._displayBuffer.WriteLine(line);
        }

        public void NotifyCommandHandlers(string input)
        {
            MonoBehaviour[] allGameObjects = FindObjectsOfType<MonoBehaviour>();
            foreach (MonoBehaviour behaviour in allGameObjects)
            {
                MethodInfo targetMethod = behaviour
                    .GetType()
                    .GetMethod("OnUserInput", BindingFlags.NonPublic | BindingFlags.Instance);
            
                if (targetMethod == null) 
                    continue;
            
                targetMethod.Invoke(behaviour, new object[] { input });
            }
        }
    }
}