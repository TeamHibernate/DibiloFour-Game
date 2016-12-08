namespace DibiloFour.Core.Commands
{
    using Interfaces;

    public class DetailsCommand : ICommand
    {
        private readonly IEngine engine;

        private readonly IOutputWriter writer;

        public DetailsCommand(IEngine engine, IOutputWriter writer)
        {
            this.engine = engine;
            this.writer = writer;
            this.Explanation = "Print details about your character.";
        }

        public string Explanation { get; private set; }

        public void Execute(string[] args)
        {
            var currentPlayer = this.engine.CurrentlyActivePlayer;
            this.writer.WriteLine(currentPlayer.Details());
        }
    }
}
