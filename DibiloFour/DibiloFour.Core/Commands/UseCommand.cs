namespace DibiloFour.Core.Commands
{

    using System;
    using System.Linq;
    using System.Text;
    using Attributes;
    using Data;

    using Interfaces;
    using Models.Dibils;

    public class UseCommand : Command
    {
        [Inject]
        private readonly DibiloFourContext context;
        [Inject]
        private Player currentPlayer;
        [Inject]
        private readonly IOutputWriter writer;
        [Inject]
        private readonly IInputReader reader;

        public UseCommand(string[] data) : base(data)
        {
            this.Explanation = "List inventory items to use. Usage - use [id]";
        }

        public override Player Execute()
        {
            if (this.currentPlayer == null)
            {
                throw new InvalidOperationException("Must be logged in");
            }

            if (this.Data.Length == 0)
            {
                this.writer.WriteLine(this.ListPlayerInventoryItems());
                return this.currentPlayer;
            }

            int id;
            var isValidNumber = int.TryParse(this.Data[0], out id);

            if (!isValidNumber)
            {
                throw new Exception("Id must be a number");
            }
            
            this.ApplyInventoryItem(id);

            return this.currentPlayer;
        }
        

        private string ListPlayerInventoryItems()
        {
            int currentPlayerInventoryId = this.currentPlayer.InventoryId;
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
            var player = this.currentPlayer;
            var item = player.Inventory.Content.FirstOrDefault(i => i.Id == itemId);

            if (item == null)
            {
                throw new ArgumentException("Player doesnt have item with id " + itemId);
            }

            item.Use(player);
        }
    }
}
