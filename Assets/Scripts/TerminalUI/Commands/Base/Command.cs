using System.Collections;

namespace TerminalUI.Commands
{
    /// <summary>
    /// Dit is de basisklasse voor ieder commando.
    /// </summary>
    public abstract class Command : ICommand
    {
        public abstract IEnumerator Run(params string[] args);
    }
}