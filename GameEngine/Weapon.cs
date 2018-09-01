using System;

namespace AdventureGameEngine
{
    public abstract class Weapon : InventoryItem
    {
        private Random damageRandomizer = new Random(DateTime.Now.Second);

        public Weapon(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer) :
            base(consoleOut, soundPlayer)
        {
            this.MinDamage = 0;
        }

        public override string Description
        {
            get;
        }

        public int MinDamage
        {
            get;
            protected set;
        }

        public int MaxDamage
        {
            get;
            protected set;
        }

        public override void DisplayTakeMessage()
        {
            this.consoleOut.WriteLine(string.Format("You now have a {0}. Don't hurt yourself with it!", this.Name));
        }

        public int Use(Character character)
        {
            var damage = this.damageRandomizer.Next(this.MinDamage, this.MaxDamage);
            character.DecreaseHealth(damage);
            return damage;
        }
    }
}