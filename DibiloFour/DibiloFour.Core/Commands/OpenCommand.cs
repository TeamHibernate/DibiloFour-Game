namespace DibiloFour.Core.Commands
{

    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using Data;

    using Interfaces;

    public class OpenCommand : ICommand
    {
        private readonly DibiloFourContext context;

        private readonly IEngine engine;
        
        private readonly IOutputWriter writer;

        public OpenCommand(DibiloFourContext context, IEngine engine, IOutputWriter writer)
        {
            this.context = context;
            this.engine = engine;
            this.writer = writer;
            this.Explanation = "List chests nearby.";
        }

        public string Explanation
        {
            get; private set;
        }

        public void Execute(string[] args)
        {
            if (this.engine.CurrentlyActivePlayer == null)
            {
                throw new InvalidOperationException("Must be logged in");
            }

            bool containChests = this.DoesThisPlayerLocationContainChests();

            if (!containChests)
            {
                this.writer.WriteLine("No chests here.");
                return;
            }

            if (args.Length == 0)
            {
                this.writer.WriteLine(this.ListChestsInCurrentPlayerLocation());
                this.writer.WriteLine("Usage: open id. Example: open 1");
                return;
            }

            int chestId;
            var isValidNumber = int.TryParse(args[0], out chestId);

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
        }

        private bool DoesThisPlayerLocationContainChests()
        {
            int currentPlayerLocationId = this.engine.CurrentlyActivePlayer.CurrentLocationId.Value;
            var chests = this.context.Chests.Where(i => i.LocationId == currentPlayerLocationId);

            if (!chests.Any())
            {
                return false;
            }

            return true;
        }

        private string ListChestsInCurrentPlayerLocation()
        {
            int currentPlayerLocationId = this.engine.CurrentlyActivePlayer.CurrentLocationId.Value;
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
            return chestLock.SkillLevelRequired <= this.engine.CurrentlyActivePlayer.LockpickingSkill;
        }

        private void PlayerTakeChestItems(int chestId)
        {
            var chestInventory = this.context.Chests.Include(c => c.Inventory)
                .First(i => i.Id == chestId)
                .Inventory.Content.ToList();

            foreach (var item in chestInventory)
            {
                item.InventoryId = (int) this.engine.CurrentlyActivePlayer.InventoryId;
                this.writer.WriteLine($"Item {item.Name} added to your inventory.");
            }

            this.context.SaveChanges();
        }
    }
}
