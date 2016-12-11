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

        private readonly CommandsManager commandsManager;
        #endregion

        #region Constructor
        public Engine(IInputReader inputReader, IOutputWriter outputWriter)
        {
            this.InputReader = inputReader;
            this.OutputWriter = outputWriter;
            this.context = new DibiloFourContext();
            this.commandsManager = new CommandsManager(this.context, this.inputReader, this.outputWriter);
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
                    .Trim();

                this.outputWriter.ClearScreen();

                try
                {
                    this.commandsManager.Execute(input);
                }
                catch (InvalidOperationException exception)
                {
                    this.outputWriter.WriteLine(exception.Message);
                    this.commandsManager.Execute("Help");
                }
            }
        }

        private void StartGameInstructions()
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine("NewGame");
            output.AppendLine("LoadGame");
            output.AppendLine("Exit");

            this.OutputWriter.Write(output.ToString());
        }

        private void WelcomeScreen()
        {
            this.OutputWriter.WriteLine("#### Dibilo Four ####");
        }

        #endregion
    }
}