namespace DibiloFour.Core.Commands
{

    using System;
    using System.Threading;
    using Attributes;
    using Data;
    using DibiloFour.Core.Interfaces;
    using Models.Dibils;

    public class ExitCommand : Command
    {
        [Inject]
        private readonly DibiloFourContext context;
        [Inject]
        private Player currentPlayer;
        [Inject]
        private readonly IOutputWriter writer;
        [Inject]
        private readonly IInputReader reader;

        public ExitCommand(string[] data) : base(data)
        {
            this.Explanation = "Exits the game";
        }

        public override Player Execute()
        {
            this.writer.WriteLine("Bye, bye :)");
            Thread.Sleep(1000);
            Environment.Exit(0);

            return this.currentPlayer;
        }
    }
}
