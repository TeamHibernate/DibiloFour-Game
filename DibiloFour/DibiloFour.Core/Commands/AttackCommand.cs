namespace DibiloFour.Core.Commands
{

    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using Interfaces;
    using Models.Dibils;

    public class AttackCommand : ICommand
    {
        private readonly DibiloFourContext context;

        private readonly Player activePlayer;

        private readonly IOutputWriter writer;

        private readonly IInputReader reader;

        public AttackCommand(DibiloFourContext context, Player activePlayer, IOutputWriter writer, IInputReader reader)
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
            //TODO: Test

            var enemy = this.context.Dibils.FirstOrDefault(d => d.Id == characterId);

            if (enemy == null)
            {
                throw new ArgumentException("Cant found enemy with id " + characterId);
            }

            //TODO: opravi go be skapanqk :))) 

            //var playerAttack = this.activePlayer.CurrentWeapon.Effect;

            //enemy.Health -= Math.Max(playerAttack - enemy.CurrentArmour.Effect, 0);

            this.writer.WriteLine("You attacked " + enemy.Name);

            if (enemy.Health > 0)
            {
                this.writer.WriteLine(enemy.Health + " health is " + enemy.Health);
            }
            else
            {
                this.writer.WriteLine(enemy.Name + " is dead");

                var enemyCoins = enemy.Coins;
                var enemyItems = enemy.Inventory.Content.ToList();

                this.activePlayer.Coins += enemyCoins;
                enemy.Coins = 0;
                
                enemyItems.ForEach(this.activePlayer.Inventory.Content.Add);
                enemy.Inventory.Content.Clear();

                this.writer.WriteLine("You picked up:");
                this.writer.WriteLine(enemyCoins + " Coints");

                if (enemyItems.Count > 0)
                {
                    this.writer.WriteLine("Items:");
                    enemyItems.Select(i => i.Name).ToList()
                        .ForEach(this.writer.WriteLine);
                }                
            }

            this.context.SaveChanges();
        }

    }
}
