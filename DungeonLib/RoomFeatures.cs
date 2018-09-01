using System;
using AdventureGameEngine;

namespace Dungeon
{
    public class DeadPool : RoomFeature
    {
        public DeadPool()
        {
            this.Name = "Pool";
        }

        public override string Description
        {
            get
            {
                return "A pool of water that may or may not be healthy to drink";
            }
        }
    }

    public class WaterPool : RoomFeature, IItemProvider
    {
        private IConsoleOutputService consoleOut;
        private ISoundPlayerService soundPlayer;

        public WaterPool(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer)
        {
            this.consoleOut = consoleOut;
            this.soundPlayer = soundPlayer;
            this.Name = "Pool";
        }

        public override string Description
        {
            get
            {
                return "A pool of clear water";
            }
        }

        public InventoryItem FindItemByName(string itemName)
        {
            if (itemName.ToLower() == "water")
            {
                return new Water(this.consoleOut, this.soundPlayer);
            }
            return null;
        }

        public InventoryItem TakeItemByName(string itemName)
        {
            if (itemName.ToLower() == "water")
            {
                return new Water(this.consoleOut, this.soundPlayer);
            }
            return null;
        }
    }

    public class TreasureChest : RoomFeature, IItemProvider
    {
        private IConsoleOutputService consoleOut;
        private ISoundPlayerService soundPlayer;

        public TreasureChest(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer, int value)
        {
            this.consoleOut = consoleOut;
            this.soundPlayer = soundPlayer;
            this.Name = "TreasureChest";
            this.Value = value;
        }

        public override string Description
        {
            get
            {
                return "A chest containing gold and other valuable items";
            }
        }

        public int Value
        {
            get;
            private set;
        }

        public InventoryItem FindItemByName(string itemName)
        {
            if (itemName.ToLower() == "gold")
            {
                var value = this.Value;
                return new GoldCoin(this.consoleOut, this.soundPlayer, value);

            }
            return null;
        }

        public InventoryItem TakeItemByName(string itemName)
        {
            if (itemName.ToLower() == "gold")
            {
                var value = this.Value;
                this.Value = 0;
                return new GoldCoin(this.consoleOut, this.soundPlayer, value);

            }
            return null;
        }
    }

    public class Skeleton : RoomFeature, IItemProvider
    {
        private Rune rune;

        public Skeleton(Rune rune)
        {
            this.Name = "Skeleton";
            this.rune = rune;
        }

        public override string Description
        {
            get
            {
                return "A skeleton in the corner of the room - a rune in its bony hand";
            }
        }

        public InventoryItem FindItemByName(string itemName)
        {
            if (itemName.ToLower() == "rune")
            {
                return this.rune;
            }
            return null;
        }

        public InventoryItem TakeItemByName(string itemName)
        {
            InventoryItem item = null;

            if (itemName.ToLower() == "rune")
            {
                item = this.rune;
                this.rune = null;
            }

            return item;
        }
    }
}
