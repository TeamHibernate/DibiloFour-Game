namespace DibiloFour.Core.Commands
{

    using System.Linq;
    using System.Text;

    using Interfaces;
    using Models;

    public class BuyCommand : ICommand
    {
        private readonly DibiloFourContext context;

        private readonly Dibil activePlayer;

        private readonly IInputReader reader;

        private readonly IOutputWriter writer;

        public BuyCommand(DibiloFourContext context, Dibil activePlayer, IInputReader reader, IOutputWriter writer)
        {
            this.context = context;
            this.activePlayer = activePlayer;
            this.reader = reader;
            this.writer = writer;
            this.Explanation = "If in item shop location list shop inventory.";
        }

        public string Explanation { get; private set; }

        public void Execute(string[] args)
        {
            if (this.HaveHereShops())
            {
                this.writer.WriteLine(this.ListItemShopInventoryItems());
                int itemToBuyId = this.GetIdFromInput();
                bool haveBoughtItem = this.TryToBuyItem(itemToBuyId);
                if (!haveBoughtItem)
                {
                    this.writer.WriteLine("You do not enough money.");
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


        private string ListItemShopInventoryItems()
        {
            int currentPlayerLocationId = this.activePlayer.CurrentLocationId.Value;
            int currentItemShopInventoryId = this.context.ItemShops
                .Where(id => id.LocationId == currentPlayerLocationId)
                .Select(inventory => inventory.InventoryId)
                .FirstOrDefault();
            var items = this.context.Inventories.FirstOrDefault(i => i.Id == currentItemShopInventoryId);
            var output = new StringBuilder();

            foreach (var item in items.Content)
            {
                output.AppendLine($"Id: {item.Id}, Name: {item.Name}");
                output.AppendLine($"Description: {item.Description}");
                output.AppendLine($"Effect: {item.Effect}, Price: {item.ValueInCoin}");
            }

            return output.ToString();
        }

        private bool TryToBuyItem(int itemToBuyId)
        {
            var shop =
                this.context.ItemShops.FirstOrDefault(
                    s => s.LocationId == this.activePlayer.CurrentLocationId);

            var wantedItem = shop.Inventory.Content.FirstOrDefault(item => item.Id == itemToBuyId);
            if (this.activePlayer.Coins >= wantedItem.ValueInCoin)
            {
                this.activePlayer.Inventory.Content.Add(wantedItem);
                shop.Inventory.Content.Remove(wantedItem);

                this.context.SaveChanges();

                return true;
            }

            return false;
        }

        private int GetIdFromInput()
        {
            //TODO: Твърде много се повтаря, ще е хубаво да е в някакъв отделен клас Utils?
            this.writer.WriteLine("Input Id: ");

            int id = int.Parse(this.reader.ReadLine());

            return id;
        }
    }
}
