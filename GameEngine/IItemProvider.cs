using System;
using System.Collections.Generic;

namespace AdventureGameEngine
{
    /// <summary>
    /// Interface to objects that provide inventory items.
    /// </summary>
    public interface IItemProvider
    {
        /// <summary>
        /// Takes an item by name from the provider.
        /// </summary>
        /// <param name="itemName">
        /// Name of the item to take.
        /// </param>
        /// <returns>
        /// The item.
        /// </returns>
        /// <remarks>
        /// Once an item is taken, the provider no
        /// longer has it.
        /// </remarks>
        InventoryItem TakeItemByName(string itemName);
    }
}
