namespace DibiloFour.Models.Items
{
    using Dibils;
    using Enums;

    public abstract class Arming : Item
    {
        public Arming(int id, string name, string description, Material material, int weight, decimal value)
            : base(id, name, description, value, weight)
        {
            this.MaterialType = material;
        }

        public Material MaterialType { get; set; }

        public override void Use(Dibil dibil)
        {
            dibil.CurrentWeapon = this;
            dibil.Inventory.CarryWeight += this.Weight;
        }

        public virtual void Disuse(Dibil dibil)
        {
            dibil.CurrentWeapon = null;
            dibil.Inventory.CarryWeight -= this.Weight;
        }
    }
}
