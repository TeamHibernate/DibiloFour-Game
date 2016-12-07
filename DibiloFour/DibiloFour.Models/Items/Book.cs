namespace DibiloFour.Models.Items
{

    using System.ComponentModel.DataAnnotations.Schema;

    using Dibils;

    [Table("Books")]
    public class Book : Stuff
    {
        //TODO: add more specifics
        public Book(int id, string name, string description, int weight, decimal value) 
            : base(id, name, description, weight, value)
        {
        }

        public int BonusLockpickingSkills { get; set; }

        public int BonusSpeechSkills { get; set; }

        public override void Use(Dibil dibil)
        {
            base.Use(dibil);
            Player player = dibil as Player;
            player.LockpickingSkill += this.BonusLockpickingSkills;
            player.SpeechSkill += this.BonusSpeechSkills;
        }
    }
}
