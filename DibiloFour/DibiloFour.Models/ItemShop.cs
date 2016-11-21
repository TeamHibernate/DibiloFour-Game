namespace DibiloFour.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ItemShop
    {
        #region Fields
        #endregion

        #region Constructor
        public ItemShop()
        {
            
        }

        public ItemShop(string name, int sellerId, int locationId, int inventoryId)
        {
            this.Name = name;
            this.SellerId = sellerId;
            this.LocationId = locationId;
            this.InventoryId = inventoryId;
        }

        public ItemShop(int id, string name, int sellerId, int locationId, int inventoryId)
        {
            this.Id = id;
            this.Name = name;
            this.SellerId = sellerId;
            this.LocationId = locationId;
            this.InventoryId = inventoryId;
        }
        #endregion

        #region Properties
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// SellerId represents Dibil that sell in the ItemShop
        /// his SpeechSkill along with buyer SpeechSkill and
        /// Item base value make the final price of Item
        /// </summary>
        [ForeignKey("Seller")]
        public int? SellerId { get; set; }

        public Dibil Seller { get; set; }

        [ForeignKey("Location")]
        public int? LocationId { get; set; }

        public Location Location { get; set; }

        [ForeignKey("Inventory")]
        public int InventoryId { get; set; }

        public Inventory Inventory { get; set; }
        #endregion
    }
}