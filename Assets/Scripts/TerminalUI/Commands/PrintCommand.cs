using System.Collections;
using TerminalUI.Commands.Base;
using WM2000.Terminal;
using Yarn;

namespace TerminalUI.Commands
{
    /// <summary>
    /// Dit commando print een tekst naar de primaire Terminal.
    /// </summary>
    [Alias("PrintLine", "Write", "WriteLine")]
    public class PrintCommand : Command
    {
        public override IEnumerator Run(params string[] args)
        {
            foreach (string arg in args)
            {
                Value result = Wm2000VariableStorage.Instance.GetValue(arg);

                Terminal.Write(result.type == Value.Type.Null ? $"{arg} " : result.AsString);
            }
            
            if (args.Length > 0)
                Terminal.WriteLine();

            yield break;
        }
    }
}