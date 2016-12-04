namespace DibiloFour.Models.Items
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;
    using Dibils;

    public abstract class Item
    {
        #region Fields
        #endregion

        #region Constructor

        protected Item()
        {
        }

        protected Item(int id, string name, string description, decimal value, int weight)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Value = value;
            this.Weight = weight;
        }
        #endregion

        #region Properties
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public int Weight { get; set; }

        [Required]
        public decimal Value { get; set; }

        [ForeignKey("Inventory")]
        public int InventoryId { get; set; }

        public Inventory Inventory { get; set; }

        #endregion

        public abstract void Use(Dibil dibil);

        public override string ToString()
        {
            var output = new StringBuilder();
            
            output.AppendLine($"Id: {this.Id}, Name: {this.Name}");
            output.AppendLine($"Description: {this.Description}");
            output.AppendLine($"Price: {this.Value}");

            return output.ToString();
        }
    }
}