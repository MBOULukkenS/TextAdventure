using System.Collections;
using WM2000.Terminal;

namespace TerminalUI.Commands
{
    public class TypeCommand : Command
    {
        public override IEnumerator Run(params string[] args)
        {
            if (args.Length == 1)
                yield return Terminal.TypeLine(Wm2000VariableStorage.Instance.GetValue(args[0]).AsString);
        }
    }
}