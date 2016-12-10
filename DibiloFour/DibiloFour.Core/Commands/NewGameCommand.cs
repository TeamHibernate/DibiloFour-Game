namespace DibiloFour.Core.Commands
{
    using Attributes;
    using Data;

    using DibiloFour.Models;

    using Interfaces;

    using Models.Dibils;

    public class NewGameCommand : Command
    {
        private const string SuccessfullyCreatedCharacter = "Successfully created character.";
        [Inject]
        private readonly DibiloFourContext context;
        [Inject]
        private Player currentPlayer;
        [Inject]
        private readonly IOutputWriter writer;
        [Inject]
        private readonly IInputReader reader;

        public NewGameCommand(string[] data) : base(data)
        {
            this.Explanation = "Creates new character";
        }

        public override Player Execute()
        {
            this.CreatePlayerCharacter();
            this.writer.WriteLine(SuccessfullyCreatedCharacter);

            return this.currentPlayer;
        }

        private void CreatePlayerCharacter()
        {
            this.writer.WriteLine("New character name:");
            var name = this.reader.ReadLine();

            var inventory = new Inventory();
            this.currentPlayer = new Player(name, inventory);

            this.context.Inventories.Add(inventory);
            this.context.Players.Add(this.currentPlayer);
            this.context.SaveChanges();
        }
    }
}
