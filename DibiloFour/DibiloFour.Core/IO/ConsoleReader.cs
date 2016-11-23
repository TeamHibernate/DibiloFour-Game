namespace DibiloFour.Core.IO
{
    using DibiloFour.Core.Interfaces;
    using System;

    public class ConsoleReader : IInputReader
    {
        public string ReadLine()
        {
            string input;
            input = Console.ReadLine();
            return input;
        }
    }
}