namespace DibiloFour.Core.IO
{
    using Interfaces;
    using System;

    public class ConsoleReader : IInputReader
    {
        public string ReadLine()
        {
            var input = Console.ReadLine();

            return input;
        }
    }
}