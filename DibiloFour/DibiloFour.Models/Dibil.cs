namespace DibiloFour.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Dibil
    {
        #region Fields
        #endregion

        #region Constructor
        public Dibil()
        {

        }

        public Dibil(string name, int health, int lockpickingSkill, int speechSkill, int coins, bool isActivePlayer)
        {
            this.Name = name;
            this.Health = health;
            this.LockpickingSkill = lockpickingSkill;
            this.SpeechSkill = speechSkill;
            this.Coins = coins;
            this.Inventory = new Inventory();
            this.IsActivePlayer = isActivePlayer;
        }

        public Dibil(int id, string name, int health, int lockpickingSkill, int speechSkill, int coins, bool isActivePlayer)
        {
            this.Id = id;
            this.Name = name;
            this.Health = health;
            this.LockpickingSkill = lockpickingSkill;
            this.SpeechSkill = speechSkill;
            this.Coins = coins;
            this.Inventory = new Inventory();
            this.IsActivePlayer = isActivePlayer;
        }

        public Dibil(int id, string name, int health, int lockpickingSkill, int speechSkill, int coins, int inventoryId, bool isActivePlayer)
        {
            this.Id = id;
            this.Name = name;
            this.Health = health;
            this.LockpickingSkill = lockpickingSkill;
            this.SpeechSkill = speechSkill;
            this.Coins = coins;
            this.InventoryId = inventoryId;
            this.IsActivePlayer = isActivePlayer;
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
        public int LockpickingSkill { get; set; }

        [Required]
        public int SpeechSkill { get; set; }

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
        
        public bool IsActivePlayer { get; set; }
        #endregion
    }
}