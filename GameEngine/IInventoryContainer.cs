using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGameEngine
{
    public interface IInventoryContainer
    {
        ObservableCollection<InventoryItem> Inventory
        {
            get;
        }
        BoolMessageResult AddToInventory(InventoryItem item);
        BoolMessageResult RemoveFromInventory(string itemName);
        InventoryItem FindItemByName(string itemName);
        IEnumerable<TItem> GetAllItems<TItem>() where TItem : InventoryItem;
        int GetItemCount<TItem>() where TItem : InventoryItem;
        TItem GetItemInUse<TItem>() where TItem : InventoryItem;
        InventoryItem UseItemByName(string itemName);
        TItem UseItem<TItem>() where TItem : InventoryItem;
        InventoryItem UseItem<TItem>(string itemName) where TItem : InventoryItem;
    }
}
