namespace DibiloFour.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class LocationType
    {
        #region Constructors
        public LocationType()
        {

        }

        public LocationType(string name)
        {
            this.Name = name;
        }

        public LocationType(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
        #endregion

        #region Properties
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }
        #endregion
    }
}