namespace DibiloFour.Core.Commands
{

    using System;
    using System.Linq;
    using System.Text;
    using Data;

    using Interfaces;

    public class UseCommand : ICommand
    {
        private readonly DibiloFourContext context;

        private readonly IEngine engine;
       
        private readonly IOutputWriter writer;

        public UseCommand(DibiloFourContext context, IEngine engine, IOutputWriter writer)
        {
            this.context = context;
            this.engine = engine;
            this.writer = writer;
            this.Explanation = "List inventory items to use. Usage - use [id]";
        }

        public string Explanation { get; private set; }

        public void Execute(string[] args)
        {
            if (this.engine.CurrentlyActivePlayer == null)
            {
                throw new InvalidOperationException("Must be logged in");
            }

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
            int currentPlayerInventoryId = this.engine.CurrentlyActivePlayer.InventoryId;
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
            var player = this.engine.CurrentlyActivePlayer;
            var item = player.Inventory.Content.FirstOrDefault(i => i.Id == itemId);

            if (item == null)
            {
                throw new ArgumentException("Player doesnt have item with id " + itemId);
            }

            item.Use(player);
        }
    }
}
