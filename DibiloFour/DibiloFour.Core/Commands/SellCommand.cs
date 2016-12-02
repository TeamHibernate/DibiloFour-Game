namespace DibiloFour.Core.Commands
{

    using System.Linq;
    using System.Text;

    using DibiloFour.Core.Interfaces;
    using DibiloFour.Models;

    public class SellCommand : ICommand
    {
        private readonly DibiloFourContext context;

        private readonly Dibil activePlayer;

        private readonly IInputReader reader;

        private readonly IOutputWriter writer;

        public SellCommand(DibiloFourContext context, Dibil activePlayer, IInputReader reader, IOutputWriter writer)
        {
            this.context = context;
            this.activePlayer = activePlayer;
            this.reader = reader;
            this.writer = writer;
            this.Explanation = "If in item shop location list player inventory.";
        }

        public string Explanation { get; private set; }

        public void Execute(string[] args)
        {
            if (this.HaveHereShops())
            {
                this.writer.WriteLine(this.ListPlayerInventoryItems());
                int itemToSellId = this.GetIdFromInput();
                bool haveSoldItem = this.TryToSellItem(itemToSellId);
                if (!haveSoldItem)
                {
                    this.writer.WriteLine("Shop owner do not have enough.");
                }
            }
            else
            {
                this.writer.WriteLine("There are not any shops near you.");
            }
        }
        
        private bool HaveHereShops()
        {
            bool locationHaveShops = this.context.ItemShops.Any(shop => shop.LocationId == this.activePlayer.CurrentLocationId);
            return locationHaveShops;
        }

        private string ListPlayerInventoryItems()
        {
            int currentPlayerInventoryId = this.activePlayer.InventoryId;
            var items = this.context.Inventories.FirstOrDefault(i => i.Id == currentPlayerInventoryId);

            StringBuilder output = new StringBuilder();

            foreach (var item in items.Content)
            {
                output.AppendLine($"Id: {item.Id}, Name: {item.Name}");
                output.AppendLine($"Description: {item.Description}");
                output.AppendLine($"Effect: {item.Effect}, Price: {item.ValueInCoin}");
            }

            return output.ToString();
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

            if (shop.MoneyBalance > wantedItem.ValueInCoin)
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
