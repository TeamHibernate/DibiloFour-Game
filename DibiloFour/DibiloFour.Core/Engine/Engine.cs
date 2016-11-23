namespace DibiloFour.Core.Engine
{
    using Interfaces;
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
                    this.CreatePlayerCharacter();
                    this.ExplainSurroundings();
                    break;
                case "help":
                    this.Help();
                    break;
                case "attack":
                    this.ListAttackableCharactersInCurrentPlayerLocation();
                    // NOTE: Probably will cause exception because it's creating object inside switch-case.
                    //       Possibly move it as argument, or put object somewhere else. Need to be tested.
                    int characterId = this.GetIdFromInput();
                    this.PlayerAttack(characterId);
                    break;
                case "goto":
                    this.ListLocations();
                    // NOTE: Probably will cause exception because it's creating object inside switch-case.
                    //       Possibly move it as argument, or put object somewhere else. Need to be tested.
                    int locationId = this.GetIdFromInput();
                    this.PlayerGoToLocation(locationId);
                    this.ExplainSurroundings();
                    break;
                case "open":
                    this.ListChestsInCurrentPlayerLocation();
                    // NOTE: Probably will cause exception because it's creating object inside switch-case.
                    //       Possibly move it as argument, or put object somewhere else. Need to be tested.
                    int chestId = this.GetIdFromInput();
                    this.PlayerTryOpenChest(chestId);
                    break;
                case "use":
                    // TODO: Use inventory
                    break;
                case "buy":
                    // TODO: Buy item from shop (if in ItemShop location).
                    break;
                case "sell":
                    // TODO: Sell item to shop (if in ItemShop location).
                    break;
                case "exit":
                    System.Environment.Exit(0);
                    break;
                default:
                    this.OutputWriter.WriteLine("Invalid command argument, type \"help\" to see valid command examples.");
                    break;
            }
        }

        private void ListChestsInCurrentPlayerLocation()
        {
            throw new NotImplementedException();
        }

        private void PlayerTryOpenChest(int chestId)
        {
            throw new NotImplementedException();
        }

        private void WellcomeScreen()
        {
            throw new NotImplementedException();
        }

        private void ListAttackableCharactersInCurrentPlayerLocation()
        {
            throw new NotImplementedException();
        }

        private void ExplainSurroundings()
        {
            throw new NotImplementedException();
        }

        private int GetIdFromInput()
        {
            throw new NotImplementedException();
        }

        private void ListLocations()
        {
            throw new NotImplementedException();
        }

        private void PlayerGoToLocation(int locationId)
        {
            throw new NotImplementedException();
        }

        private void PlayerAttack(int characterId)
        {
            throw new NotImplementedException();
        }

        private void Help()
        {
            throw new NotImplementedException();
        }

        private void CreatePlayerCharacter()
        {
            throw new NotImplementedException();
        }

        private void NewGame()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}