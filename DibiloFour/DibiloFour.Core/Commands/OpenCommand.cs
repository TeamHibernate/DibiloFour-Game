namespace DibiloFour.Core.Commands
{

    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using Attributes;
    using Data;

    using Interfaces;
    using Models.Dibils;

    public class OpenCommand : Command
    {
        [Inject]
        private readonly DibiloFourContext context;
        [Inject]
        private Player currentPlayer;
        [Inject]
        private readonly IOutputWriter writer;
        [Inject]
        private readonly IInputReader reader;

        public OpenCommand(string[] data) : base(data)
        {
            this.Explanation = "List chests nearby.";
        }

        public override Player Execute()
        {
            if (this.currentPlayer == null)
            {
                throw new InvalidOperationException("Must be logged in");
            }

            bool containChests = this.DoesThisPlayerLocationContainChests();

            if (!containChests)
            {
                this.writer.WriteLine("No chests here.");
                return this.currentPlayer;
            }

            if (this.Data.Length == 0)
            {
                this.writer.WriteLine(this.ListChestsInCurrentPlayerLocation());
                this.writer.WriteLine("Usage: Open id. Example: Open 1");
                return this.currentPlayer;
            }

            int chestId;
            var isValidNumber = int.TryParse(this.Data[0], out chestId);

            if (!isValidNumber)
            {
                throw new Exception("Id must be a valid number");
            }

            bool couldOpenChest = this.CanCurrentPlayerOpenChest(chestId);

            if (couldOpenChest)
            {
                this.PlayerTakeChestItems(chestId);
            }
            else
            {
                this.writer.WriteLine("Chest lock is too hard to lockpick.");
            }

            return this.currentPlayer;
        }

        private bool DoesThisPlayerLocationContainChests()
        {
            int currentPlayerLocationId = this.currentPlayer.CurrentLocationId.Value;
            var chests = this.context.Chests.Where(i => i.LocationId == currentPlayerLocationId);

            if (!chests.Any())
            {
                return false;
            }

            return true;
        }

        private string ListChestsInCurrentPlayerLocation()
        {
            int currentPlayerLocationId = this.currentPlayer.CurrentLocationId.Value;
            var chests = this.context.Chests.Where(i => i.LocationId == currentPlayerLocationId);
            var output = new StringBuilder();

            foreach (var chest in chests)
            {
                string lockTypeName = this.context.LockTypes.Where(i => i.Id == chest.LockTypeId).Select(n => n.Name).First();
                output.AppendLine($"Id: {chest.Id}, Lock Type: {lockTypeName}");
            }

            return output.ToString();
        }
        
        private bool CanCurrentPlayerOpenChest(int chestId)
        {
            var chest = this.context.Chests.Include(c => c.LockType).SingleOrDefault(i => i.Id == chestId);
            var chestLock = chest.LockType;
            return chestLock.SkillLevelRequired <= this.currentPlayer.LockpickingSkill;
        }

        private void PlayerTakeChestItems(int chestId)
        {
            var chestInventory = this.context.Chests.Include(c => c.Inventory)
                .First(i => i.Id == chestId)
                .Inventory.Content.ToList();

            foreach (var item in chestInventory)
            {
                item.Inventory = this.currentPlayer.Inventory;
                this.writer.WriteLine($"Item {item.Name} added to your inventory.");
            }

            this.context.SaveChanges();
        }
    }
}
