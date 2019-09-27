using System.Collections;
using TerminalUI.Commands.Base;
using WM2000.Terminal;

namespace TerminalUI.Commands
{
    /// <summary>
    /// Break, of Newline commando voor Yarn.
    /// </summary>
    [Alias("Newline")]
    public class BrCommand : Command
    {
        public override IEnumerator Run(params string[] args)
        {
            if (args.Length > 0)
                yield break;
            Terminal.WriteLine();
        }
    }
}