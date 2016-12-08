namespace DibiloFour.Core.Commands
{

    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using Interfaces;

    public class BuyCommand : ICommand
    {
        private readonly DibiloFourContext context;

        private readonly IEngine engine;

        private readonly IOutputWriter writer;

        public BuyCommand(DibiloFourContext context, IEngine engine, IOutputWriter writer)
        {
            this.context = context;
            this.engine = engine;
            this.writer = writer;
            this.Explanation = "If in item shop location list shop inventory.";
        }

        public string Explanation { get; private set; }

        public void Execute(string[] args)
        {
            if (this.engine.CurrentlyActivePlayer == null)
            {
                throw new InvalidOperationException("Must be logged in");
            }

            if (!this.HaveHereShops())
            {
                this.writer.WriteLine("There are not any shops near you.");
                return;
            }

            if (args.Length == 0)
            {
                this.writer.WriteLine(this.ListItemShopInventoryItems());
                this.writer.WriteLine("Usage buy id. Example buy 1");
                return;
            }
            
            int id;
            var isValidNumber = int.TryParse(args[0], out id);

            if (!isValidNumber)
            {
                throw new Exception("Id must be a number");
            }
            
            var haveBoughtItem = this.TryToBuyItem(id);

            if (!haveBoughtItem)
            {
                this.writer.WriteLine("You do not enough money.");
            }
            else
            {
                this.writer.WriteLine("Successfully purchased item");
            }
        }

        private bool HaveHereShops()
        {
            bool locationHaveShops = this.context.ItemShops.Any(shop => shop.LocationId == this.engine.CurrentlyActivePlayer.CurrentLocationId);
            return locationHaveShops;
        }
        
        private string ListItemShopInventoryItems()
        {
            int currentPlayerLocationId = this.engine.CurrentlyActivePlayer.CurrentLocationId.Value;
            int currentItemShopInventoryId = this.context.ItemShops
                .Where(id => id.LocationId == currentPlayerLocationId)
                .Select(inventory => inventory.InventoryId)
                .FirstOrDefault();
            var items = this.context.Inventories.FirstOrDefault(i => i.Id == currentItemShopInventoryId);
            var output = new StringBuilder();

            foreach (var item in items.Content)
            {
                output.AppendLine(item.ToString());
            }

            return output.ToString();
        }

        private bool TryToBuyItem(int itemToBuyId)
        {
            var shop =
                this.context.ItemShops.FirstOrDefault(
                    s => s.LocationId == this.engine.CurrentlyActivePlayer.CurrentLocationId);

            var wantedItem = shop.Inventory.Content.FirstOrDefault(item => item.Id == itemToBuyId);
            if (this.engine.CurrentlyActivePlayer.Coins >= wantedItem.Value)
            {
                this.engine.CurrentlyActivePlayer.Inventory.Content.Add(wantedItem);
                shop.Inventory.Content.Remove(wantedItem);

                this.context.SaveChanges();

                return true;
            }

            return false;
        }
    }
}
