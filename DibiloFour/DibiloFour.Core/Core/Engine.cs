namespace DibiloFour.Core.Core
{
    using System;
    using System.Text;
    using Interfaces;
    using Models;

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

        public TaskManager TaskManager { get; set; }
        #endregion

        #region Methods
        public void Run()
        {
            this.WelcomeScreen();

            this.TaskManager = new TaskManager(this.context, this.outputWriter, this.inputReader);
            var player = this.CurrentActivePlayerCheck();
            if (player == null)
            {
                this.TaskManager.ProcessCommand(new []{"newgame"});
            }
            else
            {
                this.TaskManager.CurrentActivePlayer = player;
            }

            this.StartGameInstructions();

            while (true)
            {
                string commandArgs = this.InputReader.ReadLine().Trim();

                this.TaskManager.ProcessCommand(commandArgs.Split());

                this.OutputWriter.WriteLine(new string('-', 50));
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

        private Dibil CurrentActivePlayerCheck()
        {
            var characters = this.context.Dibils;

            foreach (var character in characters)
            {
                if (character.IsActivePlayer == true)
                {
                    return character;
                }
            }

            return null;
        }

        #endregion
    }
}