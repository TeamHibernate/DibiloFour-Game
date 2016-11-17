namespace DibiloFour.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ItemShop
    {
        #region Fields
        #endregion

        #region Constructor
        public ItemShop()
        {

        }
        #endregion

        #region Properties
        [Key]
        public int Id { get; set; }

        // TODO: Name (of shop), DibilId that sell items (his speech skill, buyer speech skill and base item value form price)
        // shop inventory (Collection of item model class)
        // *item shop location... maybe.
        #endregion
    }
}