namespace DibiloFour.Core.Commands
{
    using System;
    using Data;
    using Interfaces;
    using Models;
     
    public class NewGameCommand : ICommand
    {
        private const string SuccessfullyCreatedCharacter = "Successfully created character.";

        private readonly DibiloFourContext context;

        private Dibil currentActivePlayer;

        private readonly IOutputWriter writer;

        private readonly IInputReader reader;

        public NewGameCommand(DibiloFourContext context, Dibil currentActivePlayer, IOutputWriter writer, IInputReader reader)
        {
            this.context = context;
            this.currentActivePlayer = currentActivePlayer;
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
            this.writer.WriteLine("New character name:");
            string name = this.reader.ReadLine();

            Dibil newPlayer = new Dibil(name, 100, 0, 0, 0)
            {
                CurrentLocationId = 1
            };

            this.currentActivePlayer = newPlayer;

            this.context.Dibils.Add(newPlayer);
            this.context.SaveChanges();
        }

        // TODO: make this a command
        //void DeletePlayer(int playerId)
        //{
        //    var player = this.context.Dibils.Find(playerId);
        //    this.context.Dibils.Remove(player);
        //    this.context.SaveChanges();
        //}
    }
}
