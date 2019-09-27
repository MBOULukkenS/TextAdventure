using System.Collections;
using Yarn.Unity;

namespace TerminalUI.Commands
{
    /// <summary>
    /// Dit commando beindigd het spel.
    /// </summary>
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