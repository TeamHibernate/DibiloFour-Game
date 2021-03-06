﻿namespace DibiloFour.Core.Commands
{

    using System;
    using System.Linq;
    using System.Text;
    using Attributes;
    using Data;
    using Interfaces;
    using Models.Dibils;

    public class BuyCommand : Command
    {
        [Inject]
        private readonly DibiloFourContext context;
        [Inject]
        private Player currentPlayer;
        [Inject]
        private readonly IOutputWriter writer;
        [Inject]
        private readonly IInputReader reader;

        public BuyCommand(string[] data) : base(data)
        { 
        }

        public override Player Execute()
        {
            if (this.currentPlayer == null)
            {
                throw new InvalidOperationException("Must be logged in");
            }

            if (!this.HaveHereShops())
            {
                this.writer.WriteLine("There are not any shops near you.");

                return this.currentPlayer;
            }

            if (this.Data.Length == 0)
            {
                this.writer.WriteLine(this.ListItemShopInventoryItems());
                this.writer.WriteLine("Usage Buy id. Example Buy 1");

                return this.currentPlayer;
            }
            
            int id;
            var isValidNumber = int.TryParse(this.Data[0], out id);

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

            return this.currentPlayer;
        }

        private bool HaveHereShops()
        {
            bool locationHaveShops = this.context.ItemShops.Any(shop => shop.LocationId == this.currentPlayer.CurrentLocationId);
            return locationHaveShops;
        }
        
        private string ListItemShopInventoryItems()
        {
            int currentPlayerLocationId = this.currentPlayer.CurrentLocationId.Value;
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
                    s => s.LocationId == this.currentPlayer.CurrentLocationId);

            var wantedItem = shop.Inventory.Content.FirstOrDefault(item => item.Id == itemToBuyId);
            if (this.currentPlayer.Coins >= wantedItem.Value)
            {
                this.currentPlayer.Inventory.Content.Add(wantedItem);
                shop.Inventory.Content.Remove(wantedItem);

                this.context.SaveChanges();

                return true;
            }

            return false;
        }
    }
}
