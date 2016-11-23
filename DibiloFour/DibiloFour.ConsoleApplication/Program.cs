namespace DibiloFour.ConsoleApplication
{
    using DibiloFour.Core.Engine;
    using DibiloFour.Core.Interfaces;
    using DibiloFour.Core.IO;

    class Program
    {
        static void Main(string[] args)
        {
            IInputReader inputReader = new ConsoleReader();
            IOutputWriter outputWriter = new ConsoleWriter();

            IEngine engine = new Engine(inputReader, outputWriter);
            engine.Run();
        }
    }
}