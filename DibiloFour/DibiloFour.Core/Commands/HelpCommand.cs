namespace DibiloFour.Core.Commands
{
    using Core;
    using Interfaces;

    public class HelpCommand : ICommand
    {
        private readonly IOutputWriter writer;

        private readonly string[] availableCommands;

        public HelpCommand(CommandsManager commandsManager, IOutputWriter writer)
        {
            this.availableCommands = commandsManager.AvailableCommands;
            this.writer = writer;
            this.Explanation = "Shows valid commands";
        }

        public string Explanation { get; private set; }

        public void Execute(string[] args)
        {
            this.writer.WriteLine("Available commands: ");
            foreach (var availableCommand in this.availableCommands)
            {
                this.writer.WriteLine(availableCommand);
            }
        }
    }
}
