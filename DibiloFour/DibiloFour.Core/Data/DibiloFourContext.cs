namespace DibiloFour.Core.Data
{
    using System.Data.Entity;
    using Models;

    public class DibiloFourContext : DbContext
    {
        public DibiloFourContext()
            : base("name=DibiloFourContext")
        {
            Database.SetInitializer<DibiloFourContext>(new DibiloFourDBInitializer());
        }

        public virtual IDbSet<Dibil> Dibils { get; set; }

        public virtual IDbSet<Chest> Chests { get; set; }

        public virtual IDbSet<Item> Items { get; set; }

        public virtual IDbSet<Inventory> Inventories { get; set; }

        public virtual IDbSet<Location> Locations { get; set; }

        public virtual IDbSet<ItemShop> ItemShops { get; set; }

        public virtual IDbSet<ItemType> ItemTypes { get; set; }

        public virtual IDbSet<LockType> LockTypes { get; set; }

        public virtual IDbSet<LocationType> LocationTypes { get; set; }
    }
}