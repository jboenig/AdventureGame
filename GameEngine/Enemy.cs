using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGameEngine
{
    public abstract class Enemy : Character, IItemProvider
    {
        public Enemy(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer) :
            base(consoleOut, soundPlayer)
        {
        }

        public InventoryItem TakeItemByName(string itemName)
        {
            InventoryItem item = null;

            if (this.IsDead())
            {
                item = this.FindItemByName(itemName);
                if (item != null)
                {
                    this.RemoveFromInventory(itemName);
                }
            }

            return item;
        }
    }
}
