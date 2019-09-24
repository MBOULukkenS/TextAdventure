using System.Collections;
using Yarn.Unity;

namespace TerminalUI.Commands
{
    public class FinishCommand : Command
    {
        public override IEnumerator Run(params string[] args)
        {
            Wm2000DialogueUi
                .Instance
                .GetComponent<DialogueRunner>()
                .Stop();
            yield return null;
        }
    }
}