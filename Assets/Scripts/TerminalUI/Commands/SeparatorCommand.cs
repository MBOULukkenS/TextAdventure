using System.Collections;
using WM2000.Terminal;

namespace TerminalUI.Commands
{
    public class SeparatorCommand : Command
    {
        public override IEnumerator Run(params string[] args)
        {
            if (args.Length > 1)
                yield break;

            if (args.Length == 1) 
                Terminal.PrintSeparator(args[0].ToCharArray(0, 1)[0]);
            else
                Terminal.PrintSeparator();
        }
    }
}