namespace DibiloFour.Core.Commands
{

    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using Data;
    using Interfaces;

    public class AttackCommand : ICommand
    {
        private readonly DibiloFourContext context;

        private readonly IEngine engine;

        private readonly IOutputWriter writer;

        private readonly IInputReader reader;

        public AttackCommand(DibiloFourContext context, IEngine engine, IOutputWriter writer, IInputReader reader)
        {
            this.context = context;
            this.engine = engine;
            this.writer = writer;
            this.reader = reader;
            this.Explanation = " List attackable characters nearby.";
        }

        public string Explanation { get; private set; }

        public void Execute(string[] args)
        {
            if (this.engine.CurrentlyActivePlayer == null)
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
            int currentPlayerLocationId = this.engine.CurrentlyActivePlayer.CurrentLocationId.Value;
            var characters = this.context.Villains.Where(i => i.CurrentLocationId == currentPlayerLocationId);

            if (!characters.Any())
            {
                return false;
            }

            return true;
        }

        private string ListAttackableCharactersInCurrentPlayerLocation()
        {
            int currentPlayerLocationId = this.engine.CurrentlyActivePlayer.CurrentLocationId.Value;
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
            if (this.engine.CurrentlyActivePlayer == null)
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
            this.engine.CurrentlyActivePlayer.Attack(enemy);

            var removedHealth = beforeHealth - enemy.Health;

            this.writer.WriteLine("You attacked " + enemy.Name);
            this.writer.WriteLine($"You inflicted {removedHealth} damage");
            this.writer.WriteLine($"{enemy.Name} now have {enemy.Health} health");

            if (enemy.Health <= 0)
            {
                this.writer.WriteLine(enemy.Name + " is dead");

                var enemyCoins = enemy.Coins;
                var enemyItems = enemy.Inventory.Content.ToList();

                this.engine.CurrentlyActivePlayer.Coins += enemyCoins;
                enemy.Coins = 0;

                var currentPlayerInventoryId = this.engine.CurrentlyActivePlayer.InventoryId;
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
