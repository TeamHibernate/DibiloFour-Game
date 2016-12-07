namespace DibiloFour.Core.Commands
{

    using Data;

    using Core;

    using DibiloFour.Models;

    using Interfaces;

    using Models.Dibils;

    public class NewGameCommand : ICommand
    {
        private const string SuccessfullyCreatedCharacter = "Successfully created character.";

        private readonly DibiloFourContext context;

        private readonly Engine engine;

        private readonly IOutputWriter writer;

        private readonly IInputReader reader;

        public NewGameCommand(DibiloFourContext context, Engine engine, IOutputWriter writer, IInputReader reader)
        {
            this.context = context;
            this.engine = engine;
            this.writer = writer;
            this.reader = reader;

            this.Explanation = "Creates new character";
        }

        public string Explanation { get; private set; }

        public void Execute(string[] args)
        {
            this.CreatePlayerCharacter();
            this.writer.WriteLine(SuccessfullyCreatedCharacter);
        }
        
        private void CreatePlayerCharacter()
        {
            this.writer.Write("New character name:");
            var name = this.reader.ReadLine();

            var inventory = new Inventory();
            this.engine.CurrentlyActivePlayer = new Player(name, inventory);

            this.context.Inventories.Add(inventory);
            this.context.Players.Add(this.engine.CurrentlyActivePlayer);
            this.context.SaveChanges();
        }
    }
}
