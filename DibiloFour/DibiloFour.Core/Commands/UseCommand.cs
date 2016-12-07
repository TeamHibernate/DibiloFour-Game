namespace DibiloFour.Core.Commands
{

    using System;
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
            this.Explanation = "List inventory items to use. Usage - use [id]";
        }

        public string Explanation { get; private set; }

        public void Execute(string[] args)
        {
            if (args.Length == 0)
            {
                this.writer.WriteLine(this.ListPlayerInventoryItems());
                return;
            }

            int id;
            var isValidNumber = int.TryParse(args[0], out id);

            if (!isValidNumber)
            {
                throw new Exception("Id must be a number");
            }
            
            this.ApplyInventoryItem(id);
        }
        

        private string ListPlayerInventoryItems()
        {
            int currentPlayerInventoryId = (int) this.activePlayer.InventoryId;
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
            var item = this.activePlayer.Inventory.Content.FirstOrDefault(i => i.Id == itemId);

            if (item == null)
            {
                throw new ArgumentException("Player doesnt have item with id " + itemId);
            }

            item.Use(this.activePlayer);
        }
    }
}
