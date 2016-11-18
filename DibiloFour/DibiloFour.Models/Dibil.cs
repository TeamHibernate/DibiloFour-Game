namespace DibiloFour.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Dibil
    {
        #region Fields
        private ICollection<Item> inventory;
        #endregion

        #region Constructor
        public Dibil(string name, int health, int lockpickingSkill, int speechSkill, int coins)
        {
            this.Name = name;
            this.Health = health;
            this.LockpickingSkill = lockpickingSkill;
            this.SpeechSkill = speechSkill;
            this.Coins = coins;
            this.inventory = new HashSet<Item>();
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

        public virtual ICollection<Item> Inventory
        {
            get
            {
                return this.inventory;
            }
            set
            {
                this.inventory = value;
            }
        }
        #endregion
    }
}