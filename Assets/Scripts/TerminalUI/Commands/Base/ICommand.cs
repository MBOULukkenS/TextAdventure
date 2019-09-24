using System.Collections;

namespace TerminalUI.Commands
{
    public interface ICommand
    {
        IEnumerator Run(params string[] args);
    }
}