namespace DibiloFour.Core.Core
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Attributes;
    using Commands;
    using Data;
    using Interfaces;
    using Models.Dibils;

    public class CommandsManager
    {
        private const string CommandsNamespacePath = "DibiloFour.Core.Commands.";
        private const string CommandSuffix = "Command";

        private const char CommandArgsDelimiter = ' ';
        private readonly DibiloFourContext context;
        private Player currentPlayer;
        private IInputReader reader;
        private IOutputWriter writer;

        public CommandsManager(DibiloFourContext context, IInputReader reader, IOutputWriter writer)
        {
            this.reader = reader;
            this.writer = writer;
            this.context = context;
        }

        public void Execute(string command)
        {
            var commandData = command
                .Split(new char[] { CommandArgsDelimiter }, StringSplitOptions.RemoveEmptyEntries);

            var commandName = commandData[0];
            string[] commandParams = commandData.Skip(1).ToArray();

            ICommand cmd = this.DispatchCommand(commandName, commandParams);

            cmd.Execute();
        }

        private ICommand DispatchCommand(string commandName, string[] commandData)
        {
            string commandFullName = GetCommandFullName(commandName);

            object[] parameters = new object[] { commandData };

            ICommand command = null;
            try
            {
                command = (Command)Activator.CreateInstance(Type.GetType(commandFullName), parameters);
            }
            catch
            {
                throw new InvalidOperationException("Invalid command!");
            }

            command = this.InjectDependencies(command);
            return command;
        }

        private string GetCommandFullName(string commandName)
        {
            return CommandsNamespacePath + commandName + CommandSuffix;
        }

        private ICommand InjectDependencies(ICommand command)
        {
            FieldInfo[] commandFields = command.GetType()
                                  .GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            FieldInfo[] dispatcherFields = typeof(CommandsManager)
                                              .GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (var field in commandFields)
            {
                var fieldAttribute = field.GetCustomAttribute(typeof(InjectAttribute));
                if (fieldAttribute != null)
                {
                    if (dispatcherFields.Any(x => x.FieldType == field.FieldType))
                    {
                        field.SetValue(command,
                            dispatcherFields.First(x => x.FieldType == field.FieldType)
                            .GetValue(this));
                    }
                }
            }

            return command;
        }

        public string GetCommandExplanation(string commandName)
        {
            var commandFullName = this.GetCommandFullName(commandName);

            var commandClass = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t => typeof(ICommand)
                    .IsAssignableFrom(t) && t.IsClass && t.FullName == commandFullName);

            var baseClass = commandClass.BaseType;
            var prop = baseClass.GetProperty("Explanation");
            var explanation = prop.Name;
           
            return explanation.ToString();
        }
    }
}
