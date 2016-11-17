namespace DibiloFour.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Dibil
    {
        #region Fields
        #endregion

        #region Constructor
        public Dibil()
        {

        }
        #endregion

        #region Properties
        [Key]
        public int Id { get; set; }

        // TODO: Name, Health, Armour, Attack, Inventory (collection of Item model class)
        // skills (lockpicking skills etc)
        #endregion
    }
}