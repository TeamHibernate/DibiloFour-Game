namespace DibiloFour.Models.Dibils
{

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using Items;

    public abstract class Dibil
    {

        #region Fields
        #endregion

        #region Constructor

        protected Dibil()
        {

        }

        protected Dibil(string name, Inventory invetory)
        {
            this.Name = name;
            this.Inventory = invetory;
        }
        #endregion

        #region Properties
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int Health { get; set; }

        [Required]
        public int Damage { get; set; }

        [Required]
        public int Armour { get; set; }
        
        [Required]
        public decimal Coins { get; set; }

        [ForeignKey("CurrentArmour")]
        public int? CurrentArmourItemId { get; set; }

        public Item CurrentArmour { get; set; }

        [ForeignKey("CurrentWeapon")]
        public int? CurrentWeaponItemId { get; set; }

        public Item CurrentWeapon { get; set; }

        [ForeignKey("CurrentLocation")]
        public int? CurrentLocationId { get; set; }

        public Location CurrentLocation { get; set; }

        [ForeignKey("Inventory")]
        public int InventoryId { get; set; }

        public Inventory Inventory { get; set; }
        #endregion

        public virtual void Attack(Dibil dibil)
        {
            dibil.AcceptAttack(this.Damage);
        }

        public virtual void AcceptAttack(int damage)
        {
            var health = this.Health - damage;

            if (this.CurrentArmour != null)
            {
                health += (int)this.CurrentArmour.Value;
            }

            this.Health = Math.Max(0, health);
        }

        public virtual string Details()
        {
            var output = new StringBuilder();

            output.AppendLine($"Name: {this.Name}");
            output.AppendLine($"Health: {this.Health}");
            output.AppendLine($"Attack: {this.Damage}");
            output.AppendLine($"Armour: {this.Armour}");
            output.AppendLine($"Coins: {this.Coins}");

            return output.ToString();
        }
    }
}