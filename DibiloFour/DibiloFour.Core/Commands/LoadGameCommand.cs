namespace DibiloFour.Core.Commands
{
    using System;
    using System.Linq;
    using System.Text;
    using Attributes;
    using Core;
    using Data;
    using Interfaces;
    using Models.Dibils;

    public class LoadGameCommand : Command
    {
        [Inject]
        private readonly DibiloFourContext context;
        [Inject]
        private Player currentPlayer;
        [Inject]
        private readonly IOutputWriter writer;
        [Inject]
        private readonly IInputReader reader;

        public LoadGameCommand(string[] data) : base(data)
        {
            this.Explanation = "Load existing character";
        }

        public override Player Execute()
        {
            if (this.Data.Length < 1)
            {
                this.writer.WriteLine(this.ListCreatedPlayers());
                this.writer.WriteLine("Usage LoadGame id. Example LoadGame 1");
                return this.currentPlayer;
            }
            
            int id;
            var isValidNumber = int.TryParse(this.Data[0], out id);

            if (!isValidNumber)
            {
                throw new ArgumentException("Invalid id");
            }

            var player = this.context.Players.FirstOrDefault(p => p.Id == id);

            if (player == null)
            {
                throw new ArgumentException($"Player with id {id} doesnt exists");
            }

            this.currentPlayer = player;
            this.writer.WriteLine($"Successfully loaded player {player.Name}");

            return this.currentPlayer;
        }

        private string ListCreatedPlayers()
        {
            var output = new StringBuilder();
            this.context.Players.Select(
                p => new
                     {
                         p.Id,
                         p.Name
                     })
                .ToList()
                .ForEach(p => output.AppendFormat("Id {0} Name {1}{2}", p.Id, p.Name, Environment.NewLine));

            return output.ToString();
        }
    }
}
