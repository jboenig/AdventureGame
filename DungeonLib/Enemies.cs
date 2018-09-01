using System;
using AdventureGameEngine;

namespace Dungeon
{
    public sealed class Troll : Enemy
    {
        private string desc;

        public Troll(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer, string name, string desc) :
            base(consoleOut, soundPlayer)
        {
            this.Name = name;
            this.desc = desc;
            this.AddToInventory(new BareHands(this.consoleOut, this.soundPlayer, 7));
        }

        public override string Description
        {
            get { return this.desc; }
        }
    }

    public sealed class Orc : Enemy
    {
        private string desc;

        public Orc(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer, string name, string desc) :
            base(consoleOut, soundPlayer)
        {
            this.Name = name;
            this.desc = desc;
            this.AddToInventory(new BareHands(this.consoleOut, this.soundPlayer, 5));
        }

        public override string Description
        {
            get { return this.desc; }
        }
    }
}
