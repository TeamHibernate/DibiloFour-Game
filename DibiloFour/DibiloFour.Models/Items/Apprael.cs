namespace DibiloFour.Models.Items
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Dibils;
    using Enums;

    [Table("Apparels")]
    public class Apprael : Arming
    {
        public Apprael()
        {
            
        }

        public Apprael(int id, string name, string description, Material material, int weight, decimal value, int bonusArmour)
            : base(id, name, description, material, weight, value)
        {
            this.BonusArmour = bonusArmour;
        }

        public int BonusArmour { get; set; }

        public override void Use(Dibil dibil)
        {
            base.Use(dibil);
            dibil.Damage -= this.BonusArmour;
        }

        public override void Disuse(Dibil dibil)
        {
            base.Disuse(dibil);
            dibil.Damage -= this.BonusArmour;
        }
    }
}
