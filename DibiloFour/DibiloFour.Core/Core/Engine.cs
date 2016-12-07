﻿namespace DibiloFour.Core.Core
{
    using System;
    using System.Linq;
    using System.Text;

    using Commands;
    using Data;
    using Interfaces;
    using Models;
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
            this.commandsManager.AddCommand(new AttackCommand(this.context, this.CurrentlyActivePlayer, this.outputWriter, this.inputReader));
            this.commandsManager.AddCommand(new BuyCommand(this.context, this.CurrentlyActivePlayer, this.inputReader, this.outputWriter));
            this.commandsManager.AddCommand(new ExitCommand(this.outputWriter));
            this.commandsManager.AddCommand(new GotoCommand(this.context, this.CurrentlyActivePlayer, this.inputReader, this.outputWriter));
            this.commandsManager.AddCommand(new OpenCommand(this.context, this.CurrentlyActivePlayer, this.inputReader, this.outputWriter));
            this.commandsManager.AddCommand(new SellCommand(this.context, this.CurrentlyActivePlayer, this.inputReader, this.outputWriter));
            this.commandsManager.AddCommand(new UseCommand(this.context, this.CurrentlyActivePlayer, this.inputReader, this.outputWriter));
            this.commandsManager.AddCommand(new HelpCommand(this.commandsManager, this.outputWriter));
        }

        public void Run()
        {
            this.WelcomeScreen();
            
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
                    this.commandsManager.Execute("Help");
                }
            }
        }
        
        private void StartGameInstructions()
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine("newgame - Create's new game.");
            output.AppendLine("loadgame - Load last saved game."); //TODO: Implement loadgame
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