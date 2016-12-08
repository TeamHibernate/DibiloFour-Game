namespace DibiloFour.Core.Core
{
    using System;
    using System.Text;

    using Commands;
    using Data;
    using Interfaces;

    using Models.Dibils;

    public class Engine : IEngine
    {
        #region Fields
        private IInputReader inputReader;
        private IOutputWriter outputWriter;
        private readonly DibiloFourContext context;

        private readonly CommandsManager commandsManager = new CommandsManager();
        #endregion

        #region Constructor
        public Engine(IInputReader inputReader, IOutputWriter outputWriter)
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

        public Player CurrentlyActivePlayer { get; set; }
        #endregion

        #region Methods
        private void InitializeCommands()
        {
            this.commandsManager.AddCommand(new NewGameCommand(this.context, this, this.outputWriter, this.inputReader));
            this.commandsManager.AddCommand(new LoadGameCommand(this, this.context, this.outputWriter));
            this.commandsManager.AddCommand(new AttackCommand(this.context, this, this.outputWriter, this.inputReader));
            this.commandsManager.AddCommand(new BuyCommand(this.context, this, this.outputWriter));
            this.commandsManager.AddCommand(new ExitCommand(this.outputWriter));
            this.commandsManager.AddCommand(new GotoCommand(this.context, this, this.outputWriter));
            this.commandsManager.AddCommand(new OpenCommand(this.context, this, this.outputWriter));
            this.commandsManager.AddCommand(new SellCommand(this.context, this, this.outputWriter));
            this.commandsManager.AddCommand(new UseCommand(this.context, this, this.outputWriter));
            this.commandsManager.AddCommand(new HelpCommand(this.commandsManager, this.outputWriter));
            this.commandsManager.AddCommand(new DetailsCommand(this, this.outputWriter));
        }

        public void Run()
        {
            if (!this.context.Database.Exists())
            {
                this.outputWriter.WriteLine("Creating database...");
                this.context.Database.Initialize(true);
            }

            this.WelcomeScreen();
            this.StartGameInstructions();

            while (true)
            {
                this.outputWriter.WriteLine(new string('-', 50));

                var input = this.inputReader.ReadLine()
                    .Trim()
                    .ToLower();

                this.outputWriter.ClearScreen();

                try
                {
                    this.commandsManager.Execute(input);
                }
                catch (Exception exception)
                {
                    this.outputWriter.WriteLine(exception.Message);
                    this.commandsManager.Execute("help");
                }
            }
        }

        private void StartGameInstructions()
        {
            StringBuilder output = new StringBuilder();

            var newGameExplanation = this.commandsManager.GetCommandExplanation("newgame");
            var loadGameExplanation = this.commandsManager.GetCommandExplanation("loadgame");
            var exitGameExplanation = this.commandsManager.GetCommandExplanation("exit");

            output.Append("newgame - ");
            output.AppendLine(newGameExplanation);

            output.Append("loadgame - ");
            output.AppendLine(loadGameExplanation);

            output.Append("exit - ");
            output.AppendLine(exitGameExplanation);

            this.OutputWriter.Write(output.ToString());
        }

        private void WelcomeScreen()
        {
            this.OutputWriter.WriteLine("#### Dibilo Four ####");
        }

        #endregion
    }
}