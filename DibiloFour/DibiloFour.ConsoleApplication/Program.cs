namespace DibiloFour.ConsoleApplication
{
    using Core.Core;
    using Core.Interfaces;
    using Core.IO;

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