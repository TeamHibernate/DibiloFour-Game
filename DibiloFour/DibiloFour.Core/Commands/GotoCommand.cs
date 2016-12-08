namespace DibiloFour.Core.Commands
{

    using System;
    using System.Linq;
    using System.Text;
    using Data;

    using DibiloFour.Core.Core;
    using DibiloFour.Core.Interfaces;
    using DibiloFour.Models;
    using Models.Dibils;

    public class GotoCommand : ICommand
    {
        private readonly DibiloFourContext context;

        private readonly IEngine engine;
        
        private readonly IOutputWriter writer;

        public GotoCommand(DibiloFourContext context, IEngine engine, IOutputWriter writer)
        {
            this.context = context;
            this.engine = engine;
            this.writer = writer;
            this.Explanation = "List locations character could go.";
        }

        public string Explanation { get; private set; }

        public void Execute(string[] args)
        {
            if (args.Length == 0)
            {
                this.writer.WriteLine(this.ListLocations());
                this.writer.WriteLine("Usage: goto id. Example: goto 1");
                return;
            }

            int locationId;
            var isValidNumber = int.TryParse(args[0], out locationId);

            if (!isValidNumber)
            {
                throw new Exception("Id must be valid number");
            }

            this.PlayerGoToLocation(locationId);
            this.writer.WriteLine(this.GetPlayerCurrentLocation());
        }

        private string ListLocations()
        {
            var locations = this.context.Locations.Select(l => new {l.Id, l.Name})
                .ToList();
            var output = new StringBuilder();

            foreach (var location in locations)
            {
                output.AppendLine($"Id: {location.Id}, Name: {location.Name}");
            }

            return output.ToString();
        }

        private void PlayerGoToLocation(int locationId)
        {
            this.engine.CurrentlyActivePlayer.CurrentLocationId = locationId;
            this.context.SaveChanges();
        }
        
        private string GetPlayerCurrentLocation()
        {
            var location = this.engine.CurrentlyActivePlayer.CurrentLocation;
            return $"You are currently located in {location.Name}, {location.Description}";
        }
    }
}
