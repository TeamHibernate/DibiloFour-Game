namespace DibiloFour.Core.IO
{
    using Interfaces;
    using System;

    public class ConsoleWriter : IOutputWriter
    {
        public void WriteLine(string output)
        {
            Console.WriteLine(output);
        }
    }
}