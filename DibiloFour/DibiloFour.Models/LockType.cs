namespace DibiloFour.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LockType
    {
        #region Fields
        #endregion

        #region Constructor
        public LockType()
        {

        }
        #endregion

        #region Properties
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int SkillLevelRequired { get; set; }
        #endregion
    }
}
