namespace DibiloFour.Core.Commands
{

    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using Data;
    using Interfaces;
    using Models;
    using Models.Dibils;

    public class OpenCommand : ICommand
    {
        private readonly DibiloFourContext context;

        private readonly Player activePlayer;

        private readonly IInputReader reader;

        private readonly IOutputWriter writer;

        public OpenCommand(DibiloFourContext context, Player activePlayer, IInputReader reader, IOutputWriter writer)
        {
            this.context = context;
            this.activePlayer = activePlayer;
            this.reader = reader;
            this.writer = writer;
            this.Explanation = "List chests nearby.";
        }

        public string Explanation { get; private set; }

        public void Execute(string[] args)
        {
            bool containChests = this.DoesThisPlayerLocationContainChests();
            if (containChests)
            {
                this.writer.WriteLine(this.ListChestsInCurrentPlayerLocation());
                int chestId = this.GetIdFromInput();
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
            else
            {
                this.writer.WriteLine("No chests here.");
            }
        }

        private bool DoesThisPlayerLocationContainChests()
        {
            int currentPlayerLocationId = this.activePlayer.CurrentLocationId.Value;
            var chests = this.context.Chests.Where(i => i.LocationId == currentPlayerLocationId);

            if (!chests.Any())
            {
                return false;
            }

            return true;
        }

        private string ListChestsInCurrentPlayerLocation()
        {
            int currentPlayerLocationId = this.activePlayer.CurrentLocationId.Value;
            var chests = this.context.Chests.Where(i => i.LocationId == currentPlayerLocationId);
            var output = new StringBuilder();
            
            foreach (var chest in chests)
            {
                string lockTypeName = this.context.LockTypes.Where(i => i.Id == chest.LockTypeId).Select(n => n.Name).FirstOrDefault();
                output.AppendLine($"Id: {chest.Id}, Lock Type: {lockTypeName}");
            }

            return output.ToString();
        }

        private int GetIdFromInput()
        {
            this.writer.WriteLine("Input Id: ");

            int id = int.Parse(this.reader.ReadLine());

            return id;
        }

        private bool CanCurrentPlayerOpenChest(int chestId)
        {
            int chestLockTypeId = this.context.Chests.Where(i => i.Id == chestId).Select(id => id.LockTypeId).SingleOrDefault();
            var chestLock = this.context.LockTypes.SingleOrDefault(i => i.Id == chestLockTypeId);
            return chestLock.SkillLevelRequired <= this.activePlayer.LockpickingSkill;
        }

        private void PlayerTakeChestItems(int chestId)
        {
            
            var chestInventory = this.context.Chests.Include(c => c.Inventory)
                .First(i => i.Id == chestId)
                .Inventory.Content.ToList();

            foreach (var item in chestInventory)
            {
                item.InventoryId = this.activePlayer.InventoryId;
                this.writer.WriteLine($"Item {item.Name} added to your inventory.");
            }

            this.context.SaveChanges();
        }
    }
}
