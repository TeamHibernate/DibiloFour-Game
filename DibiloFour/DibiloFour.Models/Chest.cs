namespace DibiloFour.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Chest
    {
        #region Fields
        #endregion

        #region Constructor
        public Chest()
        {
            
        }
        #endregion

        #region Properties
        [Key]
        public int Id { get; set; }

        [ForeignKey("LockType")]
        public int LockTypeId { get; set; }

        public LockType LockType { get; set; }

        [ForeignKey("Location")]
        public int? LocationId { get; set; }

        public Location Location { get; set; }

        [ForeignKey("Inventory")]
        public int InventoryId { get; set; }

        public Inventory Inventory { get; set; }
        #endregion
    }
}