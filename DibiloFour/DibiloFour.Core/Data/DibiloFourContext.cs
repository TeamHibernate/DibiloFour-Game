namespace DibiloFour.Core.Data
{
    using System.Data.Entity;
    using Models;
    using Models.Dibils;
    using Models.Items;

    public class DibiloFourContext : DbContext
    {
        public DibiloFourContext()
            : base("name=DibiloFourContext")
        {
            Database.SetInitializer<DibiloFourContext>(new DibiloFourDBInitializer());
        }

        public virtual IDbSet<Player> Players
        {
            get; set;
        }

        public virtual IDbSet<Villain> Villains
        {
            get; set;
        }

        public virtual IDbSet<Chest> Chests
        {
            get; set;
        }


        public virtual IDbSet<Potion> Potions
        {
            get; set;
        }

        public virtual IDbSet<Weapon> Weapons
        {
            get; set;
        }

        public virtual IDbSet<Apprael> Appraels
        {
            get; set;
        }

        public virtual IDbSet<Book> Books
        {
            get; set;
        }

        public virtual IDbSet<Inventory> Inventories
        {
            get; set;
        }

        public virtual IDbSet<Location> Locations
        {
            get; set;
        }

        public virtual IDbSet<ItemShop> ItemShops
        {
            get; set;
        }

        public virtual IDbSet<ItemType> ItemTypes
        {
            get; set;
        }

        public virtual IDbSet<LockType> LockTypes
        {
            get; set;
        }

        public virtual IDbSet<LocationType> LocationTypes
        {
            get; set;
        }
    }
}