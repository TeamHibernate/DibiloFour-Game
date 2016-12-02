namespace DibiloFour.Core.Commands
{

    using System.Linq;
    using System.Text;

    using DibiloFour.Core.Interfaces;
    using DibiloFour.Models;

    public class GotoCommand : ICommand
    {
        private readonly DibiloFourContext context;

        private readonly Dibil activePlayer;

        private readonly IInputReader reader;

        private readonly IOutputWriter writer;

        public GotoCommand(DibiloFourContext context, Dibil activePlayer, IInputReader reader, IOutputWriter writer)
        {
            this.context = context;
            this.activePlayer = activePlayer;
            this.reader = reader;
            this.writer = writer;
            this.Explanation = "List locations character could go.";
        }

        public string Explanation { get; private set; }

        public void Execute(string[] args)
        {
            this.writer.WriteLine(this.ListLocations());
            int locationId = this.GetIdFromInput();
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

        private int GetIdFromInput()
        {
            this.writer.WriteLine("Input Id: ");

            int id = int.Parse(this.reader.ReadLine());

            return id;
        }

        private void PlayerGoToLocation(int locationId)
        {
            this.activePlayer.CurrentLocationId = locationId;
            this.context.SaveChanges();
        }
        
        private string GetPlayerCurrentLocation()
        {
            int currentPlayerLocationId = this.activePlayer.CurrentLocationId.Value;
            var location = this.context.Locations.SingleOrDefault(l => l.Id == currentPlayerLocationId);
            return $"You are currently located in {location.Name}, {location.Description}";
        }
    }
}
