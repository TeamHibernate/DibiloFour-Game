namespace DibiloFour.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ItemType
    {
        #region Fields
        #endregion

        #region Constructor
        public ItemType()
        {

        }
        #endregion

        #region Properties
        [Key]
        public int Id { get; set; }

        // TODO: Name (Health Potion, Book etc.)
        #endregion
    }
}