﻿namespace DibiloFour.Core.IO
{
    using Interfaces;
    using System;

    public class ConsoleWriter : IOutputWriter
    {
        public void WriteLine(string output)
        {
            Console.WriteLine(output);
        }

        public void Write(string output)
        {
            Console.Write(output);
        }

        public void ClearScreen()
        {
            Console.Clear();
        }
    }
}