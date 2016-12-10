namespace DibiloFour.Core.Interfaces
{
    using Models.Dibils;

    public interface ICommand
    {
        string Explanation { get; }

        Player Execute();
    }
}
