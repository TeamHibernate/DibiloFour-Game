namespace DibiloFour.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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

        // TODO: Name, Info (What does it do), TypeId (heal, boost, skill book etc.), Effect (20, -20 etc.)
        // item value (base price of item that is traded in shop)
        #endregion
    }
}