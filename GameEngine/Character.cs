using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AdventureGameEngine
{
    public abstract class Character : IInventoryContainer, INotifyPropertyChanged
    {
        private int health;
        private GameBoard.Position pos;
        private ObservableCollection<InventoryItem> inventory;
        private InventoryItem itemInUse;
        protected IConsoleOutputService consoleOut;
        protected ISoundPlayerService soundPlayer;

        public Character(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer)
        {
            this.consoleOut = consoleOut;
            this.soundPlayer = soundPlayer;
            this.health = 100;
            this.pos = GameBoard.Position.Undefined;
            this.inventory = new ObservableCollection<InventoryItem>();
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

        public int Health
        {
            get { return this.health; }
        }

        public GameBoard.Position Position
        {
            get { return this.pos; }
            set { this.pos = value; }
        }

        public void DecreaseHealth(int amount)
        {
            this.health = this.health - amount;
            if (this.health < 0)
            {
                this.health = 0;
            }
            this.FirePropertyChanged("Health");
        }

        public void IncreaseHealth(int amount)
        {
            this.health = this.health + amount;
            if (this.health > 100)
            {
                this.health = 100;
            }
            this.FirePropertyChanged("Health");
        }

        public bool IsDead()
        {
            if (this.health <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DisplayHealth()
        {
            this.consoleOut.WriteLine("");
            this.consoleOut.WriteLine("Health is " + this.Health.ToString());
        }

        public ObservableCollection<InventoryItem> Inventory
        {
            get { return this.inventory; }
        }

        public BoolMessageResult AddToInventory(InventoryItem item)
        {
            if (item == null)
            {
                return new BoolMessageResult(false, "Item is null");
            }
            this.inventory.Add(item);
            item.Container = this;
            return BoolMessageResult.Success;
        }

        public BoolMessageResult RemoveFromInventory(string itemName)
        {
            int itemIdx = -1;
            for (int i = 0; i < this.inventory.Count && itemIdx == -1; i++)
            {
                if (this.inventory[i].Name.ToLower() == itemName.ToLower())
                {
                    itemIdx = i;
                }
            }

            if (itemIdx != -1)
            {
                var item = this.inventory[itemIdx];
                item.Container = null;
                this.inventory.RemoveAt(itemIdx);
                return BoolMessageResult.Success;
            }

            return new BoolMessageResult(false, "You don't have one of those to drop");
        }

        public InventoryItem FindItemByName(string itemName)
        {
            foreach (var item in this.inventory)
            {
                if (item.Name.ToLower() == itemName.ToLower())
                {
                    return item;
                }
            }
            return null;
        }

        public IEnumerable<TItem> GetAllItems<TItem>() where TItem : InventoryItem
        {
            List<TItem> items = new List<TItem>();
            foreach (var curItem in this.inventory)
            {
                var itemOfType = curItem as TItem;
                if (itemOfType != null)
                {
                    items.Add(itemOfType);
                }
            }
            return items;
        }

        public int GetItemCount<TItem>() where TItem : InventoryItem
        {
            return this.GetAllItems<TItem>().Count();
        }

        public TItem GetItemInUse<TItem>() where TItem : InventoryItem
        {
            return this.itemInUse as TItem;
        }

        public InventoryItem UseItemByName(string itemName)
        {
            return this.UseItem<InventoryItem>(itemName);
        }

        public TItem UseItem<TItem>() where TItem : InventoryItem
        {
            if (this.itemInUse is TItem)
            {
                // Current item in use is of type TItem, so
                // just return it
                return (TItem)this.itemInUse;
            }

            // If there is one item in the inventory that matches
            // the type TItem, then set it as the item in use and
            // return it. Otherwise, return null.
            List<TItem> matchingItems = new List<TItem>();
            foreach (var item in this.inventory)
            {
                var itemOfMatchingType = item as TItem;
                if (itemOfMatchingType != null)
                {
                    matchingItems.Add(itemOfMatchingType);
                }
            }

            if (matchingItems.Count == 1)
            {
                // There is exactly 1 item matching the specified type.
                // Set it as the item in use and return it.
                var matchingItem = matchingItems.First();
                var prevItem = this.itemInUse;

                this.itemInUse = matchingItem;

                if (prevItem != null)
                {
                    prevItem.FirePropertyChanged("InUse");
                }

                if (this.itemInUse != null)
                {
                    this.itemInUse.FirePropertyChanged("InUse");
                }

                return matchingItem;
            }

            return null;
        }

        public InventoryItem UseItem<TItem>(string itemName) where TItem : InventoryItem
        {
            var item = this.FindItemByName(itemName) as TItem;
            if (item != null)
            {
                var prevItem = this.itemInUse;

                this.itemInUse = item;

                if (prevItem != null)
                {
                    prevItem.FirePropertyChanged("InUse");
                }

                if (this.itemInUse != null)
                {
                    this.itemInUse.FirePropertyChanged("InUse");
                }
            }
            return item;
        }

        public void DisplayInventory()
        {
            this.consoleOut.WriteLine("");
            this.consoleOut.WriteLine("You possess the following items:");

            foreach (var item in this.inventory)
            {
                if (item == this.itemInUse)
                {
                    this.consoleOut.Write("* ");
                }
                this.consoleOut.WriteLine(string.Format("{0} - {1}", item.Name, item.Description));
            }

            this.consoleOut.WriteLine("");
        }

        public BoolMessageResult Attack(Character character)
        {
            var weapon = this.UseItem<Weapon>();
            if (weapon == null)
            {
                return new BoolMessageResult(false, "You have no weapon selected");
            }
            else
            {
                weapon.Use(character);
            }

            return BoolMessageResult.Success;
        }

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
}
