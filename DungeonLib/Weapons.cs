﻿using System;
using AdventureGameEngine;

namespace Dungeon
{
    public class BareHands : Weapon
    {
        public BareHands(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer, int maxDamage) :
            base(consoleOut, soundPlayer)
        {
            this.Name = "BareHands";
            this.MaxDamage = maxDamage;
        }

        public override string Description
        {
            get
            {
                return string.Format("Bare hands - {0} hit points", this.MaxDamage);
            }
        }

        public override int WeightInLbs
        {
            get
            {
                return 0;
            }
        }
    }

    public class Dagger : Weapon
    {
        public Dagger(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer,
            string name = "Dagger", int maxDamage = 10) :
            base(consoleOut, soundPlayer)
        {
            this.Name = name;
            this.MaxDamage = maxDamage;
        }

        public override string Description
        {
            get
            {
                return string.Format("Short but deadly - {0} hit points", this.MaxDamage);
            }
        }

        public override int WeightInLbs
        {
            get
            {
                return 4;
            }
        }
    }

    public class BattleAxe : Weapon
    {
        public BattleAxe(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer, 
            string name = "BattleAxe", int maxDamage = 15) :
            base(consoleOut, soundPlayer)
        {
            this.Name = name;
            this.MaxDamage = maxDamage;
        }

        public override string Description
        {
            get
            {
                return string.Format("A battle axe - {0} hit points", this.MaxDamage);
            }
        }

        public override int WeightInLbs
        {
            get
            {
                return 15;
            }
        }
    }

    public class Katana : Weapon
    {
        public Katana(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer,
            string name = "Katana", int maxDamage = 10) :
            base(consoleOut, soundPlayer)
        {
            this.Name = name;
            this.MaxDamage = maxDamage;
        }

        public override string Description
        {
            get
            {
                return string.Format("{0} - {1} hit points", this.Name, this.MaxDamage);
            }
        }

        public override int WeightInLbs
        {
            get
            {
                return 12;
            }
        }
    }

    class BroadSword : Weapon
    {
        public BroadSword(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer,
            string name = "BroadSword", int maxDamage = 14) :
            base(consoleOut, soundPlayer)
        {
            this.Name = name;
            this.MaxDamage = maxDamage;
        }

        public override string Description
        {
            get
            {
                return string.Format("{0} - {1} hit points", this.Name, this.MaxDamage);
            }
        }

        public override int WeightInLbs
        {
            get
            {
                return 20;
            }
        }
    }
    
    class Bow : Weapon
    {
        public Bow(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer,
            string name = "Bow", int maxDamage = 12) :
            base(consoleOut, soundPlayer)
        {
            this.Name = name;
            this.MaxDamage = maxDamage;
        }

        public override string Description
        {
            get
            {
                return string.Format("{0} - {1} hit points", this.Name, this.MaxDamage);
            }
        }

        public override int WeightInLbs
        {
            get
            {
                return 9;
            }
        }
    }

    class Staff : Weapon
    {
        public Staff(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer,
            string name = "Staff", int maxDamage = 6) :
            base(consoleOut, soundPlayer)
        {
            this.Name = name;
            this.MaxDamage = maxDamage;
        }

        public override string Description
        {
            get
            {
                return string.Format("{0} - {1} hit points", this.Name, this.MaxDamage);
            }
        }

        public override int WeightInLbs
        {
            get
            {
                return 5;
            }
        }
    }
}