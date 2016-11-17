namespace DibiloFour.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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

        // TODO: LockTypeId of lock, Inventory (collection of Item model class)
        // *chest location... maybe.
        #endregion
    }
}