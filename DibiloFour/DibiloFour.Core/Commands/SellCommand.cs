namespace DibiloFour.Core.Commands
{

    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using DibiloFour.Core.Interfaces;
    using Models.Dibils;

    public class SellCommand : ICommand
    {
        private readonly DibiloFourContext context;

        private readonly Player activePlayer;

        private readonly IInputReader reader;

        private readonly IOutputWriter writer;

        public SellCommand(DibiloFourContext context, Player activePlayer, IInputReader reader, IOutputWriter writer)
        {
            this.context = context;
            this.activePlayer = activePlayer;
            this.reader = reader;
            this.writer = writer;
            this.Explanation = "If in item shop location list player inventory.";
        }

        public string Explanation
        {
            get; private set;
        }

        public void Execute(string[] args)
        {
            if (!this.HaveHereShops())
            {
                this.writer.WriteLine("There are not any shops near you.");
                return;
            }

            if (args.Length == 0)
            {
                this.writer.WriteLine(this.ListPlayerInventoryItems());
                this.writer.WriteLine("Usage: sell id. Example: sell 1");
                return;
            }
            
            int itemToSellId;
            var isValidNumber = int.TryParse(args[0], out itemToSellId);

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
            bool locationHaveShops = this.context.ItemShops.Any(shop => shop.LocationId == this.activePlayer.CurrentLocationId);
            return locationHaveShops;
        }

        private string ListPlayerInventoryItems()
        {
            int currentPlayerInventoryId = (int) this.activePlayer.InventoryId;
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

        private int GetIdFromInput()
        {
            //TODO: Твърде много се повтаря, ще е хубаво да е в някакъв отделен клас Utils?
            this.writer.WriteLine("Input Id: ");

            int id = int.Parse(this.reader.ReadLine());

            return id;
        }

        private bool TryToSellItem(int itemToSellId)
        {
            var shop = this.context.ItemShops.FirstOrDefault(
                            s => s.LocationId == this.activePlayer.CurrentLocationId);

            var wantedItem = this.activePlayer.Inventory.Content.FirstOrDefault(item => item.Id == itemToSellId);

            if (shop.MoneyBalance > wantedItem.Value)
            {
                shop.Inventory.Content.Add(wantedItem);
                this.activePlayer.Inventory.Content.Remove(wantedItem);
                this.context.SaveChanges();

                return true;
            }

            return false;
        }
    }
}
