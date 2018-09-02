using System;

namespace AdventureGameEngine
{
    public sealed class PowerUp : InventoryItem
    {
        public PowerUp(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer) :
            base(consoleOut, soundPlayer)
        {
            this.Name = "PowerUp";
            this.HealthPoints = 10;
        }

        public override string Description
        {
            get
            {
                return string.Format("{0} health point power up", this.HealthPoints);
            }
        }

        public override int WeightInLbs
        {
            get
            {
                return 0;
            }
        }

        public int HealthPoints
        {
            get;
            set;
        }

        public override void DisplayTakeMessage()
        {
            this.consoleOut.WriteLine(string.Format("You have a power up for {0} health points", this.HealthPoints));
        }
    }
}
