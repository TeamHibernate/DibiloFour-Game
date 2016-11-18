namespace DibiloFour.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Chest
    {
        #region Fields
        private ICollection<Item> inventory;
        #endregion

        #region Constructor
        public Chest()
        {
            this.inventory = new HashSet<Item>();
        }
        #endregion

        #region Properties
        [Key]
        public int Id { get; set; }

        [ForeignKey("LockType")]
        public int LockTypeId { get; set; }

        public LockType LockType { get; set; }

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
        
        // Note: *chest location... maybe.
        #endregion
    }
}