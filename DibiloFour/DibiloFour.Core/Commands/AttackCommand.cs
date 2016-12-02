namespace DibiloFour.Core.Commands
{

    using System;
    using System.Linq;
    using System.Text;

    using Interfaces;
    using Models;

    public class AttackCommand : ICommand
    {
        private readonly DibiloFourContext context;

        private readonly Dibil activePlayer;

        private readonly IOutputWriter writer;

        private readonly IInputReader reader;

        public AttackCommand(DibiloFourContext context, Dibil activePlayer, IOutputWriter writer, IInputReader reader)
        {
            this.context = context;
            this.activePlayer = activePlayer;
            this.writer = writer;
            this.reader = reader;
            this.Explanation = " List attackable characters nearby.";
        }

        public string Explanation { get; private set; }

        public void Execute(string[] args)
        {
            if (this.DoesThisPlayerLocationContainCharacters())
            {
                this.writer.WriteLine(this.ListAttackableCharactersInCurrentPlayerLocation());
                int characterId = this.GetIdFromInput();
                this.PlayerAttack(characterId);
            }
            else
            {
                this.writer.WriteLine("No characters here.");
            }
        }

        private bool DoesThisPlayerLocationContainCharacters()
        {
            int currentPlayerLocationId = this.activePlayer.CurrentLocationId.Value;
            var characters = this.context.Dibils.Where(i => i.CurrentLocationId == currentPlayerLocationId);

            if (!characters.Any())
            {
                return false;
            }

            return true;
        }

        private string ListAttackableCharactersInCurrentPlayerLocation()
        {
            int currentPlayerLocationId = this.activePlayer.CurrentLocationId.Value;
            var characters = this.context.Dibils.Where(i => i.CurrentLocationId == currentPlayerLocationId);

            StringBuilder output = new StringBuilder();

            foreach (var character in characters)
            {
                output.AppendLine($"Id: {character.Id}, Name: {character.Name}");
            }

            return output.ToString();
        }

        private int GetIdFromInput()
        {
            this.writer.WriteLine("Input Id: ");

            int id = int.Parse(this.reader.ReadLine());

            return id;
        }

        private void PlayerAttack(int characterId)
        {
            throw new NotImplementedException();
        }

    }
}
