using System;
using System.Collections.Generic;

namespace AdventureGameEngine
{
    public sealed class Player : Character, IItemReceiver, IConversationParticipant, ILeader
    {
        private List<Character> followers = new List<Character>();

        public Player(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer) :
            base(consoleOut, soundPlayer)
        {
            this.Name = "";
            this.MaxInventoryWeightInLbs = 30;
        }

        public override string Description
        {
            get
            {
                return "";
            }
        }

        public BoolMessageResult ReceiveItem(InventoryItem item)
        {
            // Loop through every item in the inventory to
            // see if they can receive the incoming item
            // (for example, flask receiving water).
            foreach (var curItem in this.Inventory)
            {
                var itemReceiver = curItem as IItemReceiver;
                if (itemReceiver != null)
                {
                    var res = itemReceiver.ReceiveItem(item);
                    if (res.IsSuccess)
                    {
                        return BoolMessageResult.Success;
                    }
                }
            }

            // None of the items in the player's inventory
            // can receive the item, so put it directly
            // into the player's inventory. Only allow the
            // user to have one of a given item.
            if (this.FindItemByName(item.Name) != null)
            {
                return new BoolMessageResult(false, "You already have one of those");
            }

            // Check to see if item exceeds the total
            // weight the player can carry
            var excessLbs = (item.WeightInLbs + this.InventoryWeightInLbs) - this.MaxInventoryWeightInLbs;
            if (excessLbs > 0)
            {
                return new BoolMessageResult(false, string.Format("Item is {0} pounds too heavy. You need to drop something in order to carry it.", excessLbs));
            }

            this.AddToInventory(item);

            return BoolMessageResult.Success;
        }

        public int InventoryWeightInLbs
        {
            get
            {
                int value = 0;
                foreach (var curItem in this.Inventory)
                {
                    value += curItem.WeightInLbs;
                }

                return value;
            }
        }

        public int MaxInventoryWeightInLbs
        {
            get;
        }

        public void Hear(Conversation conversation, IConversationParticipant source, string dialogText)
        {
            var msg = string.Format("{0} says - {1}", source.Name, dialogText);
            this.consoleOut.WriteLine(msg);
        }

        public void Follow(Character character)
        {
            this.followers.Add(character);
        }

        public IEnumerable<Character> Followers
        {
            get { return this.followers; }
        }
    }
}
