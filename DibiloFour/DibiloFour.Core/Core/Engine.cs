namespace DibiloFour.Core.Core
{
    using System;
    using System.Linq;
    using System.Text;

    using Commands;

    using Interfaces;
    using Models;

    public class Engine : IEngine
    {
        #region Fields
        private IInputReader inputReader;
        private IOutputWriter outputWriter;
        private DibiloFourContext context;

        private readonly CommandsManager commandsManager = new CommandsManager();
        #endregion

        #region Constructor
        public Engine(
            IInputReader inputReader,
            IOutputWriter outputWriter)
        {
            this.InputReader = inputReader;
            this.OutputWriter = outputWriter;
            this.context = new DibiloFourContext();
            this.InitializeCommands();
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
        
        public Dibil CurrentlyActivePlayer
        {
            get
            {
                return this.context.Dibils.FirstOrDefault(d => d.IsActivePlayer);
            }
        }

        #endregion

        #region Methods
        void InitializeCommands()
        {
            this.commandsManager.AddCommand(new NewGameCommand(this.context, this.CurrentlyActivePlayer, this.outputWriter, this.inputReader));
            this.commandsManager.AddCommand(new AttackCommand(this.context, this.CurrentlyActivePlayer, this.outputWriter, this.inputReader));
            this.commandsManager.AddCommand(new BuyCommand(this.context, this.CurrentlyActivePlayer, this.inputReader, this.outputWriter));
            this.commandsManager.AddCommand(new ExitCommand(this.outputWriter));
            this.commandsManager.AddCommand(new GotoCommand(this.context, this.CurrentlyActivePlayer, this.inputReader, this.outputWriter));
            this.commandsManager.AddCommand(new OpenCommand(this.context, this.CurrentlyActivePlayer, this.inputReader, this.outputWriter));
            this.commandsManager.AddCommand(new SellCommand(this.context, this.CurrentlyActivePlayer, this.inputReader, this.outputWriter));
            this.commandsManager.AddCommand(new UseCommand(this.context, this.CurrentlyActivePlayer, this.inputReader, this.outputWriter));
        }

        public void Run()
        {
            this.WelcomeScreen();

            //TODO Do we actually need this?
            /* 
            var player = this.CurrentlyActivePlayer;

            if (player == null)
            {
                this.commandsManager.Execute("newgame");
                //this.TaskManager.ProcessCommand(new []{"newgame"});
            }
            */
            
            this.StartGameInstructions();
            
            while (true)
            {
                var input = this.inputReader.ReadLine()
                    .Trim()
                    .ToLower();

                try
                {
                    this.commandsManager.Execute(input);
                }
                catch (Exception exception)
                {
                    this.outputWriter.WriteLine(exception.Message);

                    this.outputWriter.WriteLine(new string('-', 50));
                    this.outputWriter.WriteLine("Available Commands");

                    foreach (var command in this.commandsManager.AvailableCommands)
                    {
                        var commandExplanation = this.commandsManager.GetCommandExplanation(command);
                        this.outputWriter.WriteLine($"{command} - {commandExplanation}");
                    }

                    this.outputWriter.WriteLine(new string('-', 50));
                }
            }
        }

        private void StartGameInstructions()
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine("newgame - Create's new game.");
            output.AppendLine("loadgame - Load last saved game.");
            output.AppendLine("exit - Save game and exit application.");

            this.OutputWriter.Write(output.ToString());
        }

        private void WelcomeScreen()
        {
            this.OutputWriter.WriteLine("#### Dibilo Four ####");
        }
        #endregion
    }
}