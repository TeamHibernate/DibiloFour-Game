namespace DibiloFour.Core.Commands
{
    using System.Linq;
    using System.Text;
    using Data;

    using Interfaces;
    using Models.Dibils;
    using Models.Items;

    public class UseCommand : ICommand
    {
        private readonly DibiloFourContext context;

        private readonly Player activePlayer;

        private readonly IInputReader reader;

        private readonly IOutputWriter writer;

        public UseCommand(DibiloFourContext context, Player activePlayer, IInputReader reader, IOutputWriter writer)
        {
            this.context = context;
            this.activePlayer = activePlayer;
            this.reader = reader;
            this.writer = writer;
            this.Explanation = "List inventory items to use.";
        }

        public string Explanation { get; private set; }

        public void Execute(string[] args)
        {
            this.writer.WriteLine(this.ListPlayerInventoryItems());
            int itemId = this.GetIdFromInput();
            this.ApplyInventoryItem(itemId);
        }

        private int GetIdFromInput()
        {
            this.writer.WriteLine("Input Id: ");

            int id = int.Parse(this.reader.ReadLine());

            return id;
        }

        private string ListPlayerInventoryItems()
        {
            int currentPlayerInventoryId = this.activePlayer.InventoryId;
            var items = this.context.Inventories.FirstOrDefault(i => i.Id == currentPlayerInventoryId);
            var output = new StringBuilder();

            foreach (var item in items.Content)
            {
                output.AppendLine(item.ToString());
            }

            return output.ToString();
        }
        
        private void ApplyInventoryItem(int itemId)
        {
            Item wantedItem = this.activePlayer.Inventory.Content.FirstOrDefault(item => item.Id == itemId);

            wantedItem.Use(this.activePlayer);
        }
    }
}
