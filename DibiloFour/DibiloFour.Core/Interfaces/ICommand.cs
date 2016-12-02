namespace DibiloFour.Core.Interfaces
{
    public interface ICommand
    {
        string Explanation { get; }
        void Execute(string[] args);
    }
}
