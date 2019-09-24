using System.Collections;

namespace TerminalUI.Commands
{
    public abstract class Command : ICommand
    {
        public abstract IEnumerator Run(params string[] args);
    }
}