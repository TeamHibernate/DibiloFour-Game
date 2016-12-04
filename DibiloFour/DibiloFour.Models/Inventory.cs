using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DibiloFour.Models
{
    using Items;

    public class Inventory
    {
        #region Fields

        private const int DefaultMaxCarryWeight = 100;
        private ICollection<Item> content;
        #endregion

        #region Constructor
        public Inventory()
        {
            this.MaxCarryWeight = DefaultMaxCarryWeight;
            this.content = new HashSet<Item>();
        }

        public Inventory(int id)
            : this()
        {
            this.Id = id;
        }
        #endregion

        #region Properties
        [Key]
        public int Id { get; set; }

        [Required]
        public int CarryWeight { get; set; }

        [Required]
        public int MaxCarryWeight { get; set; }

        public virtual ICollection<Item> Content
        {
            get
            {
                return this.content;
            }
            set
            {
                this.content = value;
            }
        }
        #endregion
    }
}