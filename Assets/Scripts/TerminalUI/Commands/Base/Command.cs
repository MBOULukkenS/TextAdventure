namespace TerminalUI.Commands
{
    public abstract class Command : ICommand
    {
        public abstract void Run(params string[] args);
    }
}