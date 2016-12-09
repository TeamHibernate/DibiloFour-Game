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
            this.Explanation = "Print details about your character.";
        }

        public override void Execute()
        {
            var currentPlayer = this.currentPlayer;
            this.writer.WriteLine(currentPlayer.Details());
        }
    }
}
