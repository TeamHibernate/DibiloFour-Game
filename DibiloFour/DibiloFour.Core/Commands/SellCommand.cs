namespace DibiloFour.Core.Commands
{

    using System;
    using System.Linq;
    using System.Text;
    using Attributes;
    using Data;
    using DibiloFour.Core.Interfaces;
    using Models.Dibils;

    public class SellCommand : Command
    {
        [Inject]
        private readonly DibiloFourContext context;
        [Inject]
        private Player currentPlayer;
        [Inject]
        private readonly IOutputWriter writer;
        [Inject]
        private readonly IInputReader reader;

        public SellCommand(string[] data) : base(data)
        {
            this.Explanation = "If in item shop location list player inventory.";
        }

        public override void Execute()
        {
            if (this.currentPlayer == null)
            {
                throw new InvalidOperationException("Must be logged in");
            }

            if (!this.HaveHereShops())
            {
                this.writer.WriteLine("There are not any shops near you.");
                return;
            }

            if (this.Data.Length == 0)
            {
                this.writer.WriteLine(this.ListPlayerInventoryItems());
                this.writer.WriteLine("Usage: sell id. Example: sell 1");
                return;
            }
            
            int itemToSellId;
            var isValidNumber = int.TryParse(this.Data[0], out itemToSellId);

            if (!isValidNumber)
            {
                throw new Exception("Id must be a valid number");
            }

            bool haveSoldItem = this.TryToSellItem(itemToSellId);
            if (!haveSoldItem)
            {
                this.writer.WriteLine("Shop owner do not have enough.");
            }
            else
            {
                this.writer.WriteLine("Successfully sold");
            }
        }

        private bool HaveHereShops()
        {
            bool locationHaveShops = this.context.ItemShops.Any(shop => shop.LocationId == this.currentPlayer.CurrentLocationId);
            return locationHaveShops;
        }

        private string ListPlayerInventoryItems()
        {
            int currentPlayerInventoryId = this.currentPlayer.InventoryId;
            var items = this.context.Inventories.FirstOrDefault(i => i.Id == currentPlayerInventoryId);

            StringBuilder output = new StringBuilder();

            foreach (var item in items.Content)
            {
                output.AppendLine();
                output.AppendLine($"Id: {item.Id}, Name: {item.Name}");
                output.AppendLine($"Description: {item.Description}");
                output.AppendLine($"Price: {item.Value}");
            }

            return output.ToString().TrimStart();
        }
        
        private bool TryToSellItem(int itemToSellId)
        {
            var shop = this.context.ItemShops.FirstOrDefault(
                            s => s.LocationId == this.currentPlayer.CurrentLocationId);

            var wantedItem = this.currentPlayer.Inventory.Content.FirstOrDefault(item => item.Id == itemToSellId);

            if (shop.MoneyBalance > wantedItem.Value)
            {
                shop.Inventory.Content.Add(wantedItem);
                this.currentPlayer.Inventory.Content.Remove(wantedItem);
                this.context.SaveChanges();

                return true;
            }

            return false;
        }
    }
}
