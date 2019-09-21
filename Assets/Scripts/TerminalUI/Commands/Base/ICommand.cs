namespace TerminalUI.Commands
{
    public interface ICommand
    {
        void Run(params string[] args);
    }
}