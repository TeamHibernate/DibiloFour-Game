namespace DibiloFour.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    public class Item
    {
        #region Fields
        #endregion

        #region Constructor
        public Item()
        {

        }

        public Item(string name, string description, int itemTypeId, int effect, decimal valueInCoin, int inventoryId)
        {
            this.Name = name;
            this.Description = description;
            this.ItemTypeId = itemTypeId;
            this.Effect = effect;
            this.ValueInCoin = valueInCoin;
            this.InventoryId = inventoryId;
        }

        public Item(int id, string name, string description, int itemTypeId, int effect, decimal valueInCoin, int inventoryId)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.ItemTypeId = itemTypeId;
            this.Effect = effect;
            this.ValueInCoin = valueInCoin;
            this.InventoryId = inventoryId;
        }
        #endregion

        #region Properties
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
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

        public override string ToString()
        {
            var output = new StringBuilder();
            
            output.AppendLine($"Id: {this.Id}, Name: {this.Name}");
            output.AppendLine($"Description: {this.Description}");
            output.AppendLine($"Effect: {this.Effect}, Price: {this.ValueInCoin}");

            return output.ToString();
        }
    }
}