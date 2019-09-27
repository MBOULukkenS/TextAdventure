using System.Collections;

namespace TerminalUI.Commands
{
    /// <summary>
    /// Ieder commando implementeert deze interface.
    /// </summary>
    public interface ICommand
    {
        IEnumerator Run(params string[] args);
    }
}