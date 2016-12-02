namespace DibiloFour.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Interfaces;

    public class CommandsManager
    {
        const char CommandArgsDelimiter = ' ';

        const int MinCommandNameLength = 3;

        public string[] AvailableCommands => this.commands.Keys.ToArray();

        readonly Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>();
        
        public void Execute(string command)
        {
            var commandData = command.Split(new char[] { CommandArgsDelimiter }, StringSplitOptions.RemoveEmptyEntries);
            var commandName = commandData[0];

            if (commandName.Length < MinCommandNameLength)
            {
                throw new ArgumentException("Command must be at least " + MinCommandNameLength + "symbols long");
            }
            
            if (!this.AvailableCommands.Contains(commandName))
            {
                throw new ArgumentException("Invalid command");
            }

            var args = commandData.Skip(1)
                .ToArray();

            this.commands[commandName].Execute(args);
        }

        /// <summary>
        /// Добавя команда която може да се извиква. Ще се използва името на класа за да я извика. 
        /// Примерно ако се въведе help, ще потърси за добавена команда от тип HelpCommand 
        /// </summary>
        /// <param name="command">Командата която искаш да добавиш</param>
        public void AddCommand(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException();
            }

            var commandType = command.GetType();
            var commandName = this.GetCommandName(commandType);
            this.commands.Add(commandName, command);
        }

        /// <summary>
        /// Премахна команда. 
        /// След като се премахне няма да може да се извиква (очевадно).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RemoveCommand<T>() where T : ICommand
        {
            var commandType = typeof(T);

            this.commands.ToList()
                .Where(c => c.Value.GetType() == commandType)
                .ToList()
                .ForEach(c => this.commands.Remove(c.Key));
        }

        public string GetCommandExplanation(string commandName) 
        {
            if (!commands.ContainsKey(commandName))
            {
                throw new ArgumentException("Command not added");
            }

            var commandExplanation = this.commands[commandName].Explanation;
            return commandExplanation;
        }

        private string GetCommandName(Type commandType)
        {
            var iCommandType = typeof(ICommand);

            if (!commandType.GetInterfaces().Contains(iCommandType))
            {
                throw new ArgumentException("Invalid command type");
            }

            var commandName = commandType.Name.ToLowerInvariant().Replace("command", "");
            return commandName;
        }
    }
}
