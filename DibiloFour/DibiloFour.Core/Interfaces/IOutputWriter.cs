namespace DibiloFour.Core.Interfaces
{
    public interface IOutputWriter
    {
        void WriteLine(string output);
        void Write(string output);

        void ClearScreen();
    }
}