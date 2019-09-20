using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using WM2000.Terminal;
using Yarn;
using Yarn.Unity;

namespace TerminalUI
{
    public class TerminalDialogueUI : DialogueUIBehaviour
    {
        public int WordsPerMinute = 120;
        private int TypingInterval => (WordsPerMinute / 60) * 1000;
        
        public override IEnumerator RunLine(Line line)
        {
            throw new System.NotImplementedException();
        }

        public override IEnumerator RunOptions(Options optionsCollection, OptionChooser optionChooser)
        {
            throw new System.NotImplementedException();
        }

        public override IEnumerator RunCommand(Command command)
        {
            throw new System.NotImplementedException();
        }

        private async Task TypeLine(string text)
        {
            foreach (char character in text)
            {
                Terminal.Write(character.ToString());
                await Task.Delay(TypingInterval);
            }
        }

        private async Task Type(string text)
        {
            
        }
    }
}