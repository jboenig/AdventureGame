using System;

namespace AdventureGameEngine
{
    /// <summary>
    /// Interface to objects that receive inventory items.
    /// </summary>
    public interface IItemReceiver
    {
        BoolMessageResult ReceiveItem(InventoryItem item);
    }
}
