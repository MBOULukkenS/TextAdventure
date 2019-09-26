using System.Collections;
using WM2000.Terminal;
using Yarn;

namespace TerminalUI.Commands
{
    public class TypeCommand : Command
    {
        public override IEnumerator Run(params string[] args)
        {
            foreach (string arg in args)
            {
                Value result = Wm2000VariableStorage.Instance.GetValue(arg);

                if (result.type == Value.Type.Null)
                    yield return Terminal.Type($"{arg} ");
                else
                    yield return Terminal.Type(result.AsString);
            }
            
            Terminal.WriteLine();
        }
    }
}