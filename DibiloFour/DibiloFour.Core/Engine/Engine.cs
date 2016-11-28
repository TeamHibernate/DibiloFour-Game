namespace DibiloFour.Core.Engine
{
    using Interfaces;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Engine : IEngine
    {
        #region Fields
        private IInputReader inputReader;
        private IOutputWriter outputWriter;
        private DibiloFourContext context;
        private Dibil currentActivePlayer;
        #endregion

        #region Constructor
        public Engine(
            IInputReader inputReader,
            IOutputWriter outputWriter)
        {
            this.InputReader = inputReader;
            this.OutputWriter = outputWriter;
            this.context = new DibiloFourContext();
        }
        #endregion

        #region Properties
        public IInputReader InputReader
        {
            get
            {
                return this.inputReader;
            }
            set
            {
                this.inputReader = value;
            }
        }

        public IOutputWriter OutputWriter
        {
            get
            {
                return this.outputWriter;
            }
            set
            {
                this.outputWriter = value;
            }
        }
        #endregion

        #region Methods
        public void Run()
        {
            this.WellcomeScreen();
            this.CurrentActivePlayerCheck();

            while (true)
            {
                string[] commandArgs = InputReader.ReadLine().Trim().Split();
                this.ProcessCommand(commandArgs);
            }
        }
        
        private void ProcessCommand(string[] commandArgs)
        {
            switch (commandArgs[0])
            {
                case "newgame":
                    this.NewGame();
                    this.ExplainSurroundings();
                    break;
                case "help":
                    this.Help();
                    break;
                case "attack":
                    bool containCharacters = this.DoesThisPlayerLocationContainCharacters();
                    if (containCharacters == true)
                    {
                        this.ListAttackableCharactersInCurrentPlayerLocation();
                        int characterId = this.GetIdFromInput();
                        this.PlayerAttack(characterId);
                    }
                    break;
                case "goto":
                    this.ListLocations();
                    int locationId = this.GetIdFromInput();
                    this.PlayerGoToLocation(locationId);
                    this.ExplainSurroundings();
                    break;
                case "open":
                    bool containChests = this.DoesThisPlayerLocationContainChests();
                    if (containChests == true)
                    {
                        this.ListChestsInCurrentPlayerLocation();

                        int chestId = this.GetIdFromInput();
                        bool couldOpenChest = this.DoesThePlayerCouldOpenChest(chestId);

                        if (couldOpenChest == true)
                        {
                            this.PlayerTakeChestItems(chestId);
                        }
                    }
                    break;
                case "use":
                    this.ListPlayerInventoryItems();
                    int itemId = this.GetIdFromInput();
                    this.ApplyInventoryItem(itemId);
                    break;
                case "buy":
                    // TODO: Need check to see if item shop is near by
                    this.ListItemShopInventoryItems();
                    int itemToBuyId = this.GetIdFromInput();
                    this.TryToBuyItem(itemToBuyId);
                    break;
                case "sell":
                    // TODO: Need check to see if item shop is near by
                    this.ListPlayerInventoryItems();
                    int itemToSellId = this.GetIdFromInput();
                    this.TryToSellItem(itemToSellId);
                    break;
                case "exit":
                    System.Environment.Exit(0);
                    break;
                default:
                    this.OutputWriter.WriteLine("Invalid command argument, type \"help\" to see valid command examples.");
                    break;
            }
        }

        private void CurrentActivePlayerCheck()
        {
            var characters = context.Dibils;

            foreach (var character in characters)
            {
                if (character.IsActivePlayer == true)
                {
                    currentActivePlayer = character;
                    break;
                }
            }

            if (currentActivePlayer == null)
            {
                this.NewGame();
            }
        }

        private void TryToSellItem(int itemToSellId)
        {
            throw new NotImplementedException();
        }

        private void TryToBuyItem(int itemToBuyId)
        {
            throw new NotImplementedException();
        }

        private void ListItemShopInventoryItems()
        {
            int currentPlayerLocationId = currentActivePlayer.CurrentLocationId.Value;
            int currentItemShopInventoryId = context.ItemShops
                .Where(id => id.LocationId == currentPlayerLocationId)
                .Select(inventory => inventory.InventoryId)
                .FirstOrDefault();
            var items = context.Inventories.Where(i => i.Id == currentItemShopInventoryId).FirstOrDefault();

            foreach (var item in items.Content)
            {
                this.OutputWriter.WriteLine($"Id: {item.Id}, Name: {item.Name}");
                this.OutputWriter.WriteLine($"Description: {item.Description}");
                this.OutputWriter.WriteLine($"Effect: {item.Effect}, Price: {item.ValueInCoin}");
            }
        }

        private void ApplyInventoryItem(int itemId)
        {
            throw new NotImplementedException();
        }

        private void ListPlayerInventoryItems()
        {
            int currentPlayerInventoryId = currentActivePlayer.InventoryId;
            var items = context.Inventories.Where(i => i.Id == currentPlayerInventoryId).FirstOrDefault();

            foreach (var item in items.Content)
            {
                this.OutputWriter.WriteLine($"Id: {item.Id}, Name: {item.Name}");
                this.OutputWriter.WriteLine($"Description: {item.Description}");
                this.OutputWriter.WriteLine($"Effect: {item.Effect}, Price: {item.ValueInCoin}");
            }
        }

        private bool DoesThisPlayerLocationContainChests()
        {
            int currentPlayerLocationId = currentActivePlayer.CurrentLocationId.Value;
            var chests = context.Chests.Where(i => i.LocationId == currentPlayerLocationId);

            if (chests.Count() == 0)
            {
                this.OutputWriter.WriteLine("No chests here.");
                return false;
            }

            return true;
        }

        private void ListChestsInCurrentPlayerLocation()
        {
            int currentPlayerLocationId = currentActivePlayer.CurrentLocationId.Value;
            var chests = context.Chests.Where(i => i.LocationId == currentPlayerLocationId);

            foreach (var chest in chests)
            {
                string lockTypeName = context.LockTypes.Where(i => i.Id == chest.LockTypeId).Select(n => n.Name).FirstOrDefault();
                this.OutputWriter.WriteLine($"Id: {chest.Id}, Lock Type: {lockTypeName}");
            }
        }

        private bool DoesThePlayerCouldOpenChest(int chestId)
        {
            int chestLockTypeId = context.Chests.Where(i => i.Id == chestId).Select(id => id.LockTypeId).SingleOrDefault();
            var chestLock = context.LockTypes.Where(i => i.Id == chestLockTypeId).SingleOrDefault();

            if (chestLock.SkillLevelRequired > currentActivePlayer.LockpickingSkill)
            {
                this.OutputWriter.WriteLine($"Chest lock is too hard to lockpick.");
                this.OutputWriter.WriteLine($"Skill level required: {chestLock.SkillLevelRequired}");
                this.OutputWriter.WriteLine($"Your lockpicking skill: {currentActivePlayer.LockpickingSkill}");
                
                return false;
            }

            return true;
        }

        private void PlayerTakeChestItems(int chestId)
        {
            int chestInventoryId = context.Chests.Where(i => i.Id == chestId).Select(id => id.Id).FirstOrDefault();
            var chestItems = context.Items.Where(i => i.InventoryId == chestInventoryId);

            foreach (var item in chestItems)
            {
                item.InventoryId = currentActivePlayer.InventoryId;
                this.OutputWriter.WriteLine($"Item {item.Name} added to your inventory.");
            }

            this.SaveGame();
        }

        private void WellcomeScreen()
        {
            this.OutputWriter.WriteLine("#### Dibilo Four ####");
        }

        private bool DoesThisPlayerLocationContainCharacters()
        {
            int currentPlayerLocationId = currentActivePlayer.CurrentLocationId.Value;
            var characters = context.Dibils.Where(i => i.CurrentLocationId == currentPlayerLocationId);

            if (characters.Count() == 0)
            {
                this.OutputWriter.WriteLine("No characters here.");
                return false;
            }

            return true;
        }

        private void ListAttackableCharactersInCurrentPlayerLocation()
        {
            int currentPlayerLocationId = currentActivePlayer.CurrentLocationId.Value;
            var characters = context.Dibils.Where(i => i.CurrentLocationId == currentPlayerLocationId);

            foreach (var character in characters)
            {
                this.OutputWriter.WriteLine($"Id: {character.Id}, Name: {character.Name}");
            }
        }

        private void ExplainSurroundings()
        {
            int currentPlayerLocationId = currentActivePlayer.CurrentLocationId.Value;
            var location = context.Locations.Where(l => l.Id == currentPlayerLocationId).SingleOrDefault();

            this.OutputWriter.WriteLine($"You are currently located in {location.Name}, {location.Description}");
        }

        private int GetIdFromInput()
        {
            this.OutputWriter.WriteLine("Input Id:");
            int id = int.Parse(this.InputReader.ReadLine());

            return id;
        }

        private void ListLocations()
        {
            var locations = context.Locations;

            foreach (var location in locations)
            {
                this.OutputWriter.WriteLine($"Id: {location.Id}, Name: {location.Name}");
            }
        }

        private void PlayerGoToLocation(int locationId)
        {
            this.currentActivePlayer.CurrentLocationId = locationId;
            this.SaveGame();
        }

        private void PlayerAttack(int characterId)
        {
            throw new NotImplementedException();
        }

        private void Help()
        {
            this.OutputWriter.WriteLine("newgame - Create's new game.");
            this.OutputWriter.WriteLine("attack - List attackable characters nearby.");
            this.OutputWriter.WriteLine("goto - List locations character could go.");
            this.OutputWriter.WriteLine("open - List chests nearby.");
            this.OutputWriter.WriteLine("use - List inventory items to use.");
            this.OutputWriter.WriteLine("buy - If in item shop location list shop inventory.");
            this.OutputWriter.WriteLine("sell - If in item shop location list player inventory.");
            this.OutputWriter.WriteLine("exit - Save game and exit application.");
        }

        private void CreatePlayerCharacter()
        {
            this.OutputWriter.WriteLine("New character name:");
            string name = this.InputReader.ReadLine();

            Dibil newPlayer = new Dibil(name, 100, 0, 0, 0, true)
            {
                CurrentLocationId = 1
            };
            this.currentActivePlayer = newPlayer;

            context.Dibils.Add(newPlayer);
            this.SaveGame();
        }

        private void SaveGame()
        {
            context.SaveChanges();
        }

        private void NewGame()
        {
            // TODO: if no active player continue, else delete current player and reload database and seed

            this.CreatePlayerCharacter();
        }
        #endregion
    }
}