namespace DibiloFour.Models.Items
{
    using Dibils;

    public abstract class Stuff : Item
    {
        protected Stuff()
        {
                
        }

        protected Stuff(int id, string name, string description, int weight, decimal value)
            : base(id, name, description, value, weight)
        {      
        }

        public override void Use(Dibil dibil)
        {
            dibil.Inventory.CarryWeight -= this.Weight;
        }
    }
}
