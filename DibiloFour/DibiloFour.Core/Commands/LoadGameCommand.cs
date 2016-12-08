namespace DibiloFour.Core.Commands
{
    using System;
    using System.Linq;
    using System.Text;

    using Core;
    using Data;
    using Interfaces;

    public class LoadGameCommand : ICommand
    {
        private readonly IEngine engine;

        private readonly DibiloFourContext context;

        private readonly IOutputWriter writer;

        public LoadGameCommand(IEngine engine, DibiloFourContext context, IOutputWriter writer)
        {
            this.engine = engine;
            this.context = context;
            this.writer = writer;
        }

        public string Explanation => "Load existing character";

        public void Execute(string[] args)
        {
            if (args.Length < 1)
            {
                this.writer.WriteLine(this.ListCreatedPlayers());
                this.writer.WriteLine("Usage loadgame id. Example loadgame 1");
                return;
            }
            
            int id;
            var isValidNumber = int.TryParse(args[0], out id);

            if (!isValidNumber)
            {
                throw new ArgumentException("Invalid id");
            }

            var player = this.context.Players.FirstOrDefault(p => p.Id == id);

            if (player == null)
            {
                throw new ArgumentException($"Player with id {id} doesnt exists");
            }

            this.engine.CurrentlyActivePlayer = player;
            this.writer.WriteLine($"Successfully loaded player {player.Name}");
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
