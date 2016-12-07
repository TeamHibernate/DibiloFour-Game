namespace DibiloFour.Models.Dibils
{
    using System.ComponentModel.DataAnnotations;

    public class Player : Dibil
    {
        private const int DefaultHealth = 100;
        private const int DefaultMoney = 1000;
        private const int DefaultDamage = 8;
        private const int DefaultArmour = 0;

        private const int DefaultLockpickingSkill = 0;

        private const int DefaultSpeechSkill = 10;

        public Player()
        {
            
        }

        public Player(string name, Inventory inventory) :
            base(name, inventory)
        {
            this.Health = DefaultHealth;
            this.Damage = DefaultDamage;
            this.Armour = DefaultArmour;
            this.Coins = DefaultMoney;
            this.LockpickingSkill = DefaultLockpickingSkill;
            this.SpeechSkill = DefaultSpeechSkill;
        }

        [Required]
        public int LockpickingSkill { get; set; }

        [Required]
        public int SpeechSkill { get; set; }
    }
}
