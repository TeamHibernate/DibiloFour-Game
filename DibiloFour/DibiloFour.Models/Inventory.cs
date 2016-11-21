using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DibiloFour.Models
{
    public class Inventory
    {
        #region
        private ICollection<Item> content;
        #endregion

        #region Constructor
        public Inventory()
        {
            this.content = new HashSet<Item>();
        }

        public Inventory(int id)
        {
            this.Id = id;
            this.content = new HashSet<Item>();
        }
        #endregion

        #region Properties
        [Key]
        public int Id { get; set; }

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