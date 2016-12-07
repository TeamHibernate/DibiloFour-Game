namespace DibiloFour.Core.Commands
{
    using Core;
    using Interfaces;

    public class HelpCommand : ICommand
    {
        private readonly CommandsManager commandsManager;

        private readonly IOutputWriter writer;

        public HelpCommand(CommandsManager commandsManager, IOutputWriter writer)
        {
            this.commandsManager = commandsManager;
            this.writer = writer;
            this.Explanation = "Shows valid commands";
        }

        public string Explanation { get; private set; }

        public void Execute(string[] args)
        {
            this.writer.WriteLine(new string('-', 50));
            this.writer.WriteLine("Available Commands");

            foreach (var command in this.commandsManager.AvailableCommands)
            {
                var commandExplanation = this.commandsManager.GetCommandExplanation(command);
                this.writer.WriteLine($"{command} - {commandExplanation}");
            }

            this.writer.WriteLine(new string('-', 50));
        }
    }
}
