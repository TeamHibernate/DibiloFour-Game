namespace DibiloFour.Core.Commands
{
    using System.Linq;
    using System.Reflection;
    using Attributes;
    using Data;
    using Interfaces;
    using Models.Dibils;
    using System;

    public class HelpCommand : Command
    {
        [Inject]
        private readonly DibiloFourContext context;
        [Inject]
        private Player currentPlayer;
        [Inject]
        private readonly IOutputWriter writer;
        [Inject]
        private readonly IInputReader reader;

        public HelpCommand(string[] data) :base(data)
        {
            this.Explanation = "Shows valid commands";
        }

        public override Player Execute()
        {
            this.writer.WriteLine(new string('-', 50));
            this.writer.WriteLine("Available Commands: ");

            var commandClasses = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(ICommand)
                .IsAssignableFrom(t) && t.IsClass)
                .ToList();
            foreach (var command in commandClasses)
            {
                if (command.Name != "Command")
                {
                    string commandName = command.Name.Remove(command.Name.Length - 7);
                    this.writer.WriteLine($"{commandName}");
                }
            }

            this.writer.WriteLine(new string('-', 50));

            return this.currentPlayer;
        }
    }
}
