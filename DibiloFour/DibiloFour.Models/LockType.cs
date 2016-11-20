namespace DibiloFour.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LockType
    {
        #region Fields
        #endregion

        #region Constructors
        public LockType()
        {

        }

        public LockType(string name, int skillLevelRequired)
        {
            this.Name = name;
            this.SkillLevelRequired = skillLevelRequired;
        }

        public LockType(int id, string name, int skillLevelRequired)
        {
            this.Id = id;
            this.Name = name;
            this.SkillLevelRequired = skillLevelRequired;
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
