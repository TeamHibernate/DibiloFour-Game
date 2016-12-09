namespace DibiloFour.Core.Commands
{
    using System;
    using System.Linq;
    using System.Text;
    using Attributes;
    using Data;

    using DibiloFour.Core.Interfaces;
    using DibiloFour.Models;
    using Models.Dibils;

    public class GotoCommand : Command
    {
        [Inject]
        private readonly DibiloFourContext context;
        [Inject]
        private Player currentPlayer;
        [Inject]
        private readonly IOutputWriter writer;
        [Inject]
        private readonly IInputReader reader;

        public GotoCommand(string[] data) : base(data)
        {
            this.Explanation = "List locations character could go.";
        }

        public override void Execute()
        {
            if (this.Data.Length == 0)
            {
                this.writer.WriteLine(this.ListLocations());
                this.writer.WriteLine("Usage: goto id. Example: goto 1");
                return;
            }

            int locationId;
            var isValidNumber = int.TryParse(this.Data[0], out locationId);

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
            var location = this.context.Locations.FirstOrDefault(l => l.Id == locationId);

            if (location == null)
            {
                throw new ArgumentException($"Location with id {locationId} doesnt exists");
            }

            this.currentPlayer.CurrentLocation = location;
            this.context.SaveChanges();
        }
        
        private string GetPlayerCurrentLocation()
        {
            var location = this.currentPlayer.CurrentLocation;
            return $"You are currently located in {location.Name}, {location.Description}";
        }
    }
}
