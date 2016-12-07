namespace DibiloFour.Core.Commands
{

    using System;
    using System.Linq;

    using DibiloFour.Core.Core;
    using DibiloFour.Core.Data;
    using DibiloFour.Core.Interfaces;

    public class LoadGameCommand : ICommand
    {
        private readonly Engine engine;

        private readonly DibiloFourContext context;

        private readonly IOutputWriter writer;

        public LoadGameCommand(Engine engine, DibiloFourContext context, IOutputWriter writer)
        {
            this.engine = engine;
            this.context = context;
            this.writer = writer;
        }

        public string Explanation => "Doesnt need explanation";

        public void Execute(string[] args)
        {
            if (args.Length < 1)
            {
                this.ListCreatedPlayers();
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

        void ListCreatedPlayers()
        {
            this.context.Players.Select(
                p => new
                {
                    p.Id,
                    p.Name
                })
                .ToList()
                .ForEach(p => this.writer.WriteLine($"Id: {p.Id} Name: {p.Name}"));
        }
    }
}
