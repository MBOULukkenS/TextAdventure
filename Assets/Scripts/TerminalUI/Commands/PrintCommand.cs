using System.Collections;
using WM2000.Terminal;

namespace TerminalUI.Commands
{
    public class PrintCommand : Command
    {
        public override IEnumerator Run(params string[] args)
        {
            if (args.Length == 1)
                Terminal.WriteLine(Wm2000VariableStorage.Instance.GetValue(args[0]).AsString);
            yield return null;
        }
    }
}