namespace DibiloFour.Core.Commands
{

    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using Attributes;
    using Data;
    using Interfaces;
    using Models.Dibils;

    public class AttackCommand : Command
    {
        [Inject]
        private readonly DibiloFourContext context;
        [Inject]
        private Player currentPlayer;
        [Inject]
        private readonly IOutputWriter writer;
        [Inject]
        private readonly IInputReader reader;

        public AttackCommand(string[] data) : base(data)
        {
            this.Explanation = " List attackable characters nearby.";
        }

        public override void Execute()
        {
            if (this.currentPlayer == null)
            {
                throw new InvalidOperationException("Not logged in.");
            }

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
            int currentPlayerLocationId = this.currentPlayer.CurrentLocationId.Value;
            var characters = this.context.Villains.Where(i => i.CurrentLocationId == currentPlayerLocationId);

            if (!characters.Any())
            {
                return false;
            }

            return true;
        }

        private string ListAttackableCharactersInCurrentPlayerLocation()
        {
            int currentPlayerLocationId = this.currentPlayer.CurrentLocationId.Value;
            var characters = this.context.Villains.Where(i => i.CurrentLocationId == currentPlayerLocationId);

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
            if (this.currentPlayer == null)
            {
                throw new InvalidOperationException("Must be logged in");
            }

            var enemy = this.context.Villains.Include(v => v.Inventory)
                .FirstOrDefault(d => d.Id == characterId);

            if (enemy == null)
            {
                throw new ArgumentException("Cant found enemy with id " + characterId);
            }

            var beforeHealth = enemy.Health;
            this.currentPlayer.Attack(enemy);

            var removedHealth = beforeHealth - enemy.Health;

            this.writer.WriteLine("You attacked " + enemy.Name);
            this.writer.WriteLine($"You inflicted {removedHealth} damage");
            this.writer.WriteLine($"{enemy.Name} now have {enemy.Health} health");

            if (enemy.Health <= 0)
            {
                this.writer.WriteLine(enemy.Name + " is dead");

                var enemyCoins = enemy.Coins;
                var enemyItems = enemy.Inventory.Content.ToList();

                this.currentPlayer.Coins += enemyCoins;
                enemy.Coins = 0;

                var currentPlayerInventoryId = this.currentPlayer.InventoryId;
                var currentPlayerInventory = this.context.Inventories.First(i => i.Id == currentPlayerInventoryId);

                enemyItems.ForEach(currentPlayerInventory.Content.Add);
                enemy.Inventory.Content.Clear();

                this.writer.WriteLine("You picked up:");
                this.writer.WriteLine(enemyCoins + " Coints");

                if (enemyItems.Count > 0)
                {
                    this.writer.WriteLine("Items:");
                    enemyItems.Select(i => i.Name)
                        .ToList()
                        .ForEach(this.writer.WriteLine);
                }
            }

            this.context.SaveChanges();
        }

    }
}
