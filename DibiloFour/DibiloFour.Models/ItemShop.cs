namespace DibiloFour.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ItemShop
    {
        #region Fields
        private ICollection<Item> inventory;
        #endregion

        #region Constructor
        public ItemShop()
        {
            this.inventory = new HashSet<Item>();
        }
        #endregion

        #region Properties
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, ForeignKey("Seller")]
        public int SellerId { get; set; }

        public Dibil Seller { get; set; }

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

        // Note: Dibil sell items (his speech skill, buyer speech skill and base item value form price)
        // *item shop location... maybe.
        #endregion
    }
}