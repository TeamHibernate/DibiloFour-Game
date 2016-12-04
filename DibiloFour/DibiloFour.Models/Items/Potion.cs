namespace DibiloFour.Models.Items
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Dibils;

    [Table("Potions")]
    public class Potion : Stuff
    {
        // TODO: add more specifics
        public Potion(int id, string name, string description, int weight, decimal value, int healthValue)
            : base(id, name, description, weight, value)
        {
            this.BonusHealthValue = healthValue;
        }

        public int BonusHealthValue { get; set; }

        public override void Use(Dibil dibil)
        {
            base.Use(dibil);

            dibil.Health += this.BonusHealthValue;
        }
    }
}
