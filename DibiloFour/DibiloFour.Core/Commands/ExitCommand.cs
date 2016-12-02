namespace DibiloFour.Core.Commands
{

    using System;
    using System.Threading;

    using DibiloFour.Core.Interfaces;

    public class ExitCommand : ICommand
    {
        private readonly IOutputWriter writer;

        public ExitCommand(IOutputWriter writer)
        {
            this.writer = writer;
            this.Explanation = "Save game and exit application.";
        }

        public string Explanation { get; private set; }

        public void Execute(string[] args)
        {
            this.writer.WriteLine("Bye, bye :)");
            Thread.Sleep(1000);
            Environment.Exit(0);
        }
    }
}
