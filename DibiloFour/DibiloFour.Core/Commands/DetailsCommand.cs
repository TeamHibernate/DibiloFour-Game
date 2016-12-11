namespace DibiloFour.Core.Commands
{
    using Attributes;
    using Data;
    using Interfaces;
    using Models.Dibils;

    public class DetailsCommand : Command
    {
        [Inject]
        private readonly DibiloFourContext context;
        [Inject]
        private Player currentPlayer;
        [Inject]
        private readonly IOutputWriter writer;
        [Inject]
        private readonly IInputReader reader;

        public DetailsCommand(string[] data) : base(data)
        {
        }

        public override Player Execute()
        {
            this.writer.WriteLine(currentPlayer.Details());

            return this.currentPlayer;
        }
    }
}
