using Yarn.Unity;

namespace TerminalUI.Commands
{
    public class FinishCommand : Command
    {
        public override void Run(params string[] args)
        {
            Wm2000DialogueUi
                .Instance
                .GetComponent<DialogueRunner>()
                .Stop();
        }
    }
}