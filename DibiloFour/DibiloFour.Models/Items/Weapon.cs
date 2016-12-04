namespace DibiloFour.Models.Items
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Dibils;
    using Enums;

    [Table("Weapons")]
    public class Weapon : Arming
    {
        public Weapon(int id, string name, string description, Material material, int weight, int value, int bonusDamage)
            : base(id, name, description, material, weight, value)
        {
            this.BonusDamage = bonusDamage;
        }

        public int BonusDamage { get; set; }

        public override void Use(Dibil dibil)
        {
            base.Use(dibil);
            dibil.Damage -= this.BonusDamage;
        }

        public override void Disuse(Dibil dibil)
        {
            base.Disuse(dibil);
            dibil.Damage -= this.BonusDamage;
        }
    }
}
