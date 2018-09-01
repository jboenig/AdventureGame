using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AdventureGameEngine;

namespace Dungeon
{
    class Dog : Friend, IConversationParticipant
    {
        public Dog(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer, string name, string desc) :
            base(consoleOut, soundPlayer, name, desc)
        {
        }

        public void Hear(Conversation conversation, IConversationParticipant source, string dialogText)
        {
            if (!(source is Player))
            {
                return;
            }

            if (dialogText.Contains("good girl"))
            {
                conversation.Say(this.Name, "Of course I am!");
            }
            else if (dialogText.Contains("follow"))
            {
                conversation.Say(this.Name, "Oh Boy Oh Boy Oh Boy Oh Boy!!!!");
                var leader = source as ILeader;
                if (leader != null)
                {
                    leader.Follow(this);
                }
            }
            else if (dialogText.Contains("attack"))
            {
                conversation.Say(this.Name, "Wimper, wimper, wimper...right behind you boss - way behind you");
                var leader = source as ILeader;
                if (leader != null)
                {
                    leader.Follow(this);
                }
            }
            else
            {
                var responseNum = DateTime.Now.Second % 5;
                if (responseNum == 0)
                {
                    conversation.Say(this.Name, "Woof!");
                }
                else if (responseNum == 1)
                {
                    conversation.Say(this.Name, "Grrrr!");
                }
                else if (responseNum == 3)
                {
                    conversation.Say(this.Name, "Barkity bark bark!");
                }
                else if (responseNum == 4)
                {
                    conversation.Say(this.Name, "Wazzup!!");
                }
            }
        }
    }

    class Dwarf : Friend, IItemProvider, IConversationParticipant
    {
        public Dwarf(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer, string name, string desc) :
            base(consoleOut, soundPlayer, name, desc)
        {
        }

        public InventoryItem TakeItemByName(string itemName)
        {
            InventoryItem item = null;

            if (!string.IsNullOrEmpty(itemName))
            {
                var rune = this.FindItemByName(itemName) as Rune;
                if (rune != null)
                {
                    var res = this.RemoveFromInventory(itemName);
                    if (res.IsSuccess)
                    {
                        item = rune;
                    }
                }
            }

            return item;
        }

        public void Hear(Conversation conversation, IConversationParticipant source, string dialogText)
        {
            if (!(source is Player))
            {
                return;
            }

            if (dialogText.Contains("rune"))
            {
                var runes = this.GetAllItems<Rune>();
                if (runes.Count() > 1)
                {
                    var strBuilder = new StringBuilder();
                    strBuilder.Append("Yes, I have some runes with the following markings on them - ");
                    foreach (var curRune in runes)
                    {
                        strBuilder.Append(curRune.Name);
                        strBuilder.Append(" ");
                    }
                    strBuilder.Append("\rYou are welcome to take them my friend.");
                    conversation.Say(this.Name, strBuilder.ToString());
                }
                else if (runes.Count() > 0)
                {
                    var strBuilder = new StringBuilder();
                    strBuilder.Append("Yes, I have a rune with the following markings on it - ");
                    foreach (var curRune in runes)
                    {
                        strBuilder.Append(curRune.Name);
                        strBuilder.Append(" ");
                    }
                    strBuilder.Append("\rYou are welcome to take it my friend.");
                    conversation.Say(this.Name, strBuilder.ToString());
                }
                else
                {
                    conversation.Say(this.Name, "I'm sorry friend, but I don't know anything about the runes you speak of!");
                }
            }
            else if (dialogText.Contains("help"))
            {
                conversation.Say(this.Name, "I will do my best!");
            }
            else if (dialogText.Contains("follow"))
            {
                conversation.Say(this.Name, "I will stay by your side through thick and thin!");
                var leader = source as ILeader;
                if (leader != null)
                {
                    leader.Follow(this);
                }
            }
            else if (dialogText.ToLower().StartsWith("use"))
            {
                var tokens = dialogText.ToLower().Split(new char[] { ' ' });
                if (tokens.Length > 1 && tokens[0] == "use")
                {
                    var itemName = tokens[1];
                    var item = this.UseItem<InventoryItem>(itemName);
                    if (item == null)
                    {
                        conversation.Say(this.Name, string.Format("Sorry boss, I don't have a {0}!", itemName));
                    }
                    else
                    {
                        conversation.Say(this.Name, string.Format("Aye, my {0} is at the ready!", itemName));
                    }
                }
            }
            else if (dialogText.Contains("attack"))
            {
                conversation.Say(this.Name, "Charge!");
            }
            else
            {
                var responseNum = DateTime.Now.Second % 5;
                if (responseNum == 0)
                {
                    conversation.Say(this.Name, "Do you have any gold?");
                }
                else if (responseNum == 1)
                {
                    conversation.Say(this.Name, "If you keep talking like that I won't be your friend anymore!");
                }
                else if (responseNum == 3)
                {
                    conversation.Say(this.Name, "That makes my beard itchy!");
                }
                else if (responseNum == 4)
                {
                    conversation.Say(this.Name, "Wazzup!!");
                }
            }
        }
    }
}
