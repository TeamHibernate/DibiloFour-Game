namespace DibiloFour.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Item
    {
        #region Fields
        #endregion

        #region Constructor
        public Item()
        {

        }
        #endregion

        #region Properties
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        [Required, ForeignKey("ItemType")]
        public int ItemTypeId { get; set; }

        public ItemType ItemType { get; set; }

        [Required]
        public int Effect { get; set; }

        [Required]
        public decimal ValueInCoin { get; set; }

        [ForeignKey("Inventory")]
        public int InventoryId { get; set; }

        public Inventory Inventory { get; set; }
        #endregion
    }
}