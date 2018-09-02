using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace AdventureGameEngine
{
    public abstract class InventoryItem : INotifyPropertyChanged
    {
        protected IConsoleOutputService consoleOut;
        protected ISoundPlayerService soundPlayer;

        public InventoryItem(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer)
        {
            this.consoleOut = consoleOut;
            this.soundPlayer = soundPlayer;
        }

        public string Name
        {
            get;
            protected set;
        }

        public abstract string Description
        {
            get;
        }

        public IInventoryContainer Container
        {
            get;
            set;
        }

        public bool InUse
        {
            get
            {
                bool res = false;

                if (this.Container != null)
                {
                    var itemInUse = this.Container.GetItemInUse<InventoryItem>();
                    res = (this == itemInUse);
                }

                return res;
            }
        }

        public abstract int WeightInLbs
        {
            get;
        }

        public abstract void DisplayTakeMessage();

        public event PropertyChangedEventHandler PropertyChanged;

        public void FirePropertyChanged(string propertyName)
        {
            var propChangedHandler = this.PropertyChanged;
            if (propChangedHandler != null)
            {
                var evtArgs = new PropertyChangedEventArgs(propertyName);
                propChangedHandler(this, evtArgs);
            }
        }
    }

    public class Flask : InventoryItem, IItemReceiver
    {
        private int pctFull = 50;

        public Flask(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer) :
            base(consoleOut, soundPlayer)
        {
            this.consoleOut = consoleOut;
            this.Name = "Flask";
        }

        public override string Description
        {
            get
            {
                return string.Format("{0} percent full", this.pctFull);
            }
        }

        public override int WeightInLbs
        {
            get
            {
                if (this.pctFull > 75)
                {
                    return 4;
                }
                else if (this.pctFull > 50)
                {
                    return 3;
                }
                else if (this.pctFull > 25)
                {
                    return 2;
                }
                return 1;
            }
        }

        public bool Drink(int drinkQty)
        {
            if (this.pctFull >= drinkQty)
            {
                this.pctFull = this.pctFull - drinkQty;
                this.FirePropertyChanged("Description");
                return true;
            }
            return false;
        }

        public void Add(Water water)
        {
            this.pctFull += water.FlaskPercent;
            if (this.pctFull > 100)
            {
                this.pctFull = 100;
            }
            this.FirePropertyChanged("Description");
        }

        public BoolMessageResult ReceiveItem(InventoryItem item)
        {
            var water = item as Water;
            if (water != null)
            {
                this.Add(water);
                return BoolMessageResult.Success;
            }
            var msg = string.Format("You cannot put a {0} in a flask", item.ToString());
            return new BoolMessageResult(false, msg);
        }

        public override void DisplayTakeMessage()
        {
            this.consoleOut.WriteLine("You now have a flask");
        }
    }

    public class Water : InventoryItem
    {
        public Water(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer) :
            base(consoleOut, soundPlayer)
        {
            this.Name = "Water";
        }

        public override string Description
        {
            get
            {
                return "Wonderful, clear, clean water";
            }
        }

        public override int WeightInLbs
        {
            get
            {
                return 1;
            }
        }

        public int FlaskPercent
        {
            get { return 25; }
        }

        public override void DisplayTakeMessage()
        {
            this.consoleOut.WriteLine("You now have some water");
        }
    }

    public class CoinPurse : InventoryItem, IItemReceiver
    {
        private int balance = 0;

        public CoinPurse(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer) :
            base(consoleOut, soundPlayer)
        {
            this.Name = "Purse";
        }

        public override string Description
        {
            get
            {
                return string.Format("worth {0}", this.balance);
            }
        }

        public override int WeightInLbs
        {
            get
            {
                return 5;
            }
        }

        public void Add(GoldCoin coin)
        {
            this.balance += coin.Value;
            this.soundPlayer.PlaySoundEffect("Coins");
            this.FirePropertyChanged("Description");
        }

        public BoolMessageResult ReceiveItem(InventoryItem item)
        {
            var goldCoin = item as GoldCoin;
            if (goldCoin != null)
            {
                this.Add(goldCoin);
                return BoolMessageResult.Success;
            }
            var msg = string.Format("You cannot put a {0} in a coin purse", item.ToString());
            return new BoolMessageResult(false, msg);
        }

        public override void DisplayTakeMessage()
        {
            this.consoleOut.WriteLine("You now have a purse in which to put gold coins");
        }
    }

    public class GoldCoin : InventoryItem
    {
        public GoldCoin(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer, int value = 1) :
            base(consoleOut, soundPlayer)
        {
            this.Name = "GoldCoin";
            this.Value = value;
        }

        public override string Description
        {
            get
            {
                return "Shiny gold coin";
            }
        }

        public int Value
        {
            get;
            private set;
        }

        public override int WeightInLbs
        {
            get
            {
                return 0;
            }
        }

        public override void DisplayTakeMessage()
        {
            this.consoleOut.WriteLine(string.Format("You now have gold coins worth {0}", this.Value));
        }
    }

    public class Rune : InventoryItem
    {
        private string text;

        public Rune(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer, string name, string text) :
            base(consoleOut, soundPlayer)
        {
            this.Name = name;
            this.text = text;
        }

        public override string Description
        {
            get
            {
                return "A rune containing a cryptic message";
            }
        }

        public string Text
        {
            get
            {
                return this.text;
            }
        }

        public void Read()
        {
            this.consoleOut.WriteLine(this.text);
        }

        public override int WeightInLbs
        {
            get
            {
                return 0;
            }
        }

        public override void DisplayTakeMessage()
        {
            this.consoleOut.WriteLine("You now have a rune. You might want to figure out what it means.");
        }
    }

    public sealed class RuneCollection : List<Rune>
    {
        public Rune UseNext()
        {
            Rune rune = null;

            if (this.Count > 0)
            {
                rune = this.First();
                this.RemoveAt(0);
            }

            return rune;
        }
    }
}
