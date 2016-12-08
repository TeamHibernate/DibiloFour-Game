namespace DibiloFour.Models.Dibils
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
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
    }
}