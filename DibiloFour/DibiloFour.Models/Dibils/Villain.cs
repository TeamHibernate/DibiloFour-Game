namespace DibiloFour.Models.Dibils
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Villains")]
    public class Villain : Dibil
    {
        private const int DefaultHealth = 100;
        private const int DefaultDamage = 5;
        private const int DefaultArmour = 0;
        private const int DefaultMoney = 50;

        public Villain()
        {
            
        }

        // when creating an ordinary Villain
        public Villain(int id, string name, int locationId, Inventory inventory)
            : base(name, inventory)
        {
            this.Id = id;
            this.CurrentLocationId = locationId;
            this.Health = DefaultHealth;
            this.Damage = DefaultDamage;
            this.Armour = DefaultArmour;
            this.Coins = DefaultMoney;
        }

        // when creating a special Villain
        public Villain(int id, string name, int health, int locationId, Inventory inventory) :
            base(name, inventory)
        {
            this.Id = id;
            this.Health = health;
            this.Damage = DefaultDamage;
            this.Armour = DefaultArmour;
            this.CurrentLocationId = locationId;
            this.Coins = DefaultMoney;
        }
    }
}
