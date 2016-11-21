namespace DibiloFour.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Location
    {
        #region Constructors
        public Location()
        {

        }

        public Location(string name, string description, int locationTypeId)
        {
            this.Name = name;
            this.Description = description;
            this.LocationTypeId = locationTypeId;
        }

        public Location(int id, string name, string description, int locationTypeId)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.LocationTypeId = locationTypeId;
        }
        #endregion

        #region Properties
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [ForeignKey("LocationType")]
        public int LocationTypeId { get; set; }

        public LocationType LocationType { get; set; }
        #endregion
    }
}