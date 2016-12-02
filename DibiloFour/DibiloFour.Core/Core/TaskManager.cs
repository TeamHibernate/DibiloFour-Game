namespace DibiloFour.Core.Core
{
    using System;
    using System.Data.Entity;
    using System.Text;
    using Models;
    using System.Linq;
    using System.Threading;

    using Interfaces;

    public class TaskManager
    {
        //TODO: Remove?
        #region Fields
        private readonly DibiloFourContext context;
        #endregion

        #region Constructors
        public TaskManager(DibiloFourContext context)
        {
            this.context = context;
        }

        public TaskManager(DibiloFourContext context, IOutputWriter writer, IInputReader reader)
        {
            this.context = context;
            this.Reader = reader;
            this.Writer = writer;
        }

        public TaskManager(DibiloFourContext context, Dibil currentActivePlayer)
        {
            this.context = context;
            this.CurrentActivePlayer = currentActivePlayer;
        }

        #endregion

        #region Properties
        public IInputReader Reader
        {
            get; set;
        }

        public IOutputWriter Writer
        {
            get; set;
        }

        public Dibil CurrentActivePlayer
        {
            get; set;
        }

        #endregion

        #region Methods
        internal void ProcessCommand(string[] commandArgs)
        {
            switch (commandArgs[0])
            {
                case "newgame":
                    this.NewGame();
                    this.Writer.WriteLine(this.ExplainSurroundings());
                    this.Writer.WriteLine(this.Help());
                    break;
                case "help":
                    this.Writer.WriteLine(this.Help());
                    break;
                case "attack":
                    if (this.DoesThisPlayerLocationContainCharacters())
                    {
                        this.ListAttackableCharactersInCurrentPlayerLocation();
                        int characterId = this.GetIdFromInput();
                        this.PlayerAttack(characterId);
                    }
                    else
                    {
                        this.Writer.WriteLine("No characters here.");
                    }
                    break;
                case "goto":
                    this.Writer.Write(this.ListLocations());
                    int locationId = this.GetIdFromInput();
                    this.PlayerGoToLocation(locationId);
                    this.Writer.WriteLine(this.ExplainSurroundings());
                    break;
                case "open":
                    bool containChests = this.DoesThisPlayerLocationContainChests();
                    if (containChests)
                    {
                        this.Writer.WriteLine(this.ListChestsInCurrentPlayerLocation());

                        int chestId = this.GetIdFromInput();
                        bool couldOpenChest = this.DoesThePlayerCouldOpenChest(chestId);

                        if (couldOpenChest == true)
                        {
                            this.Writer.WriteLine(this.PlayerTakeChestItems(chestId));
                        }
                    }
                    else
                    {
                        this.Writer.WriteLine("No chests here.");
                    }
                    break;
                case "use":
                    this.Writer.WriteLine(this.ListPlayerInventoryItems());
                    int itemId = this.GetIdFromInput();
                    this.ApplyInventoryItem(itemId);
                    break;
                case "buy":
                    if (this.HaveHereShops())
                    {
                        this.Writer.WriteLine(this.ListItemShopInventoryItems());
                        int itemToBuyId = this.GetIdFromInput();
                        bool haveBoughtItem = this.TryToBuyItem(itemToBuyId);
                        if (!haveBoughtItem)
                        {
                            this.Writer.WriteLine("You do not enough money.");
                        }
                    }
                    else
                    {
                        this.Writer.WriteLine("There are not any shops near you.");
                    }
                    break;
                case "sell":
                    if (this.HaveHereShops())
                    {
                        this.Writer.WriteLine(this.ListPlayerInventoryItems());
                        int itemToSellId = this.GetIdFromInput();
                        bool haveSoldItem = this.TryToSellItem(itemToSellId);
                        if (!haveSoldItem)
                        {
                            this.Writer.WriteLine("Shop owner do not have enough.");
                        }
                    }
                    else
                    {
                        this.Writer.WriteLine("There are not any shops near you.");
                    }
                    break;
                case "exit":
                    this.Writer.WriteLine("Bye, bye :)");
                    Thread.Sleep(1000);
                    Environment.Exit(0);
                    break;
                default:
                    this.Writer.WriteLine("Invalid command argument, type \"help\" to see valid command examples.");
                    break;
            }

            this.Writer.ClearScreen();
        }

        private bool HaveHereShops()
        {
            bool locationHaveShops = this.context.ItemShops.Any(shop => shop.LocationId == this.CurrentActivePlayer.CurrentLocationId);
            return locationHaveShops;
        }

        private bool TryToSellItem(int itemToSellId)
        {
            var shop = this.context.ItemShops.FirstOrDefault(
                            s => s.LocationId == this.CurrentActivePlayer.CurrentLocationId);

            var wantedItem = this.CurrentActivePlayer.Inventory.Content.FirstOrDefault(item => item.Id == itemToSellId);

            if (shop.MoneyBalance > wantedItem.ValueInCoin)
            {
                shop.Inventory.Content.Add(wantedItem);
                this.CurrentActivePlayer.Inventory.Content.Remove(wantedItem);
                this.SaveGame();

                return true;
            }

            return false;
        }

        private bool TryToBuyItem(int itemToBuyId)
        {
            var shop =
                this.context.ItemShops.FirstOrDefault(
                    s => s.LocationId == this.CurrentActivePlayer.CurrentLocationId);

            var wantedItem = shop.Inventory.Content.FirstOrDefault(item => item.Id == itemToBuyId);
            if (this.CurrentActivePlayer.Coins >= wantedItem.ValueInCoin)
            {
                this.CurrentActivePlayer.Inventory.Content.Add(wantedItem);
                shop.Inventory.Content.Remove(wantedItem);

                this.SaveGame();

                return true;
            }

            return false;
        }

        private string ListItemShopInventoryItems()
        {
            int currentPlayerLocationId = this.CurrentActivePlayer.CurrentLocationId.Value;
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

        private void ApplyInventoryItem(int itemId)
        {
            throw new NotImplementedException();
        }

        private string ListPlayerInventoryItems()
        {
            int currentPlayerInventoryId = this.CurrentActivePlayer.InventoryId;
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

        private bool DoesThisPlayerLocationContainChests()
        {
            int currentPlayerLocationId = this.CurrentActivePlayer.CurrentLocationId.Value;
            var chests = this.context.Chests.Where(i => i.LocationId == currentPlayerLocationId);

            if (!chests.Any())
            {
                return false;
            }

            return true;
        }

        private string ListChestsInCurrentPlayerLocation()
        {
            int currentPlayerLocationId = this.CurrentActivePlayer.CurrentLocationId.Value;
            var chests = this.context.Chests.Where(i => i.LocationId == currentPlayerLocationId);

            StringBuilder output = new StringBuilder();

            foreach (var chest in chests)
            {
                string lockTypeName = this.context.LockTypes.Where(i => i.Id == chest.LockTypeId).Select(n => n.Name).FirstOrDefault();

                output.AppendLine($"Id: {chest.Id}, Lock Type: {lockTypeName}");
            }

            return output.ToString();
        }

        private bool DoesThePlayerCouldOpenChest(int chestId)
        {
            int chestLockTypeId = this.context.Chests.Where(i => i.Id == chestId).Select(id => id.LockTypeId).SingleOrDefault();
            var chestLock = this.context.LockTypes.SingleOrDefault(i => i.Id == chestLockTypeId);

            if (chestLock.SkillLevelRequired > this.CurrentActivePlayer.LockpickingSkill)
            {
                this.Writer.WriteLine("Chest lock is too hard to lockpick.");
                this.Writer.WriteLine($"Skill level required: {chestLock.SkillLevelRequired}");
                this.Writer.WriteLine($"Your lockpicking skill: {this.CurrentActivePlayer.LockpickingSkill}");

                return false;
            }

            return true;
        }

        private string PlayerTakeChestItems(int chestId)
        {
            int chestInventoryId = this.context.Chests.Where(i => i.Id == chestId).Select(id => id.Id).FirstOrDefault();
            var chestItems = this.context.Items.Where(i => i.InventoryId == chestInventoryId);

            StringBuilder output = new StringBuilder();

            foreach (var item in chestItems)
            {
                item.InventoryId = this.CurrentActivePlayer.InventoryId;
                output.AppendLine($"Item {item.Name} added to your inventory.");
            }

            this.SaveGame();

            return output.ToString();
        }

        private bool DoesThisPlayerLocationContainCharacters()
        {
            int currentPlayerLocationId = this.CurrentActivePlayer.CurrentLocationId.Value;
            var characters = this.context.Dibils.Where(i => i.CurrentLocationId == currentPlayerLocationId);

            if (!characters.Any())
            {
                return false;
            }

            return true;
        }

        private void ListAttackableCharactersInCurrentPlayerLocation()
        {
            int currentPlayerLocationId = this.CurrentActivePlayer.CurrentLocationId.Value;
            var characters = this.context.Dibils.Where(i => i.CurrentLocationId == currentPlayerLocationId);

            StringBuilder output = new StringBuilder();

            foreach (var character in characters)
            {
                output.AppendLine($"Id: {character.Id}, Name: {character.Name}");
            }
        }

        private string ExplainSurroundings()
        {
            int currentPlayerLocationId = this.CurrentActivePlayer.CurrentLocationId.Value;
            var location = this.context.Locations.SingleOrDefault(l => l.Id == currentPlayerLocationId);

            return $"You are currently located in {location.Name}, {location.Description}";
        }

        private int GetIdFromInput()
        {
            this.Writer.WriteLine("Input Id: ");

            int id = int.Parse(this.Reader.ReadLine());

            return id;
        }

        private string ListLocations()
        {
            var locations = this.context.Locations;

            StringBuilder output = new StringBuilder();

            foreach (var location in locations)
            {
                output.AppendLine($"Id: {location.Id}, Name: {location.Name}");
            }

            return output.ToString();
        }

        private void PlayerGoToLocation(int locationId)
        {
            this.CurrentActivePlayer.CurrentLocationId = locationId;
            this.SaveGame();
        }

        private void PlayerAttack(int characterId)
        {
            throw new NotImplementedException();
        }

        private string Help()
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine("newgame - Create a new game.");
            output.AppendLine("attack - List attackable characters nearby.");
            output.AppendLine("goto - List locations character could go.");
            output.AppendLine("open - List chests nearby.");
            output.AppendLine("use - List inventory items to use.");
            output.AppendLine("buy - If in item shop location list shop inventory.");
            output.AppendLine("sell - If in item shop location list player inventory.");
            output.AppendLine("exit - Save game and exit application.");

            return output.ToString();
        }

        private void CreatePlayerCharacter()
        {
            // TODO: remove the writer and reader
            this.Writer.WriteLine("New character name:");
            string name = this.Reader.ReadLine();

            Dibil newPlayer = new Dibil(name, 100, 0, 0, 0, true)
            {
                CurrentLocationId = 1
            };

            this.CurrentActivePlayer = newPlayer;

            this.context.Dibils.Add(newPlayer);
            this.SaveGame();
        }

        private void SaveGame()
        {
            this.context.SaveChanges();
        }

        private void NewGame()
        {
            if (this.CurrentActivePlayer != null)
            {
                try
                {
                    this.DeletePlayer(this.CurrentActivePlayer.Id);
                    //TODO: tell client its deleted?
                }
                catch (Exception exception)
                {
                    this.Writer.WriteLine("Error while trying to delete player. Error message: " + exception.Message);
                    //TODO: show error?
                }
            }
            
            this.CreatePlayerCharacter();
        }
        #endregion

        void DeletePlayer(int playerId)
        {
            var player = this.context.Dibils.Find(playerId);
            this.context.Dibils.Remove(player);
            this.context.SaveChanges();
        }
    }
}
