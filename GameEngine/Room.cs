using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventureGameEngine
{
    /// <summary>
    /// This class represents a room on the map.
    /// </summary>
    public class Room : Tile, IItemProvider
    {
        private IConsoleOutputService consoleOut;

        public Room(IConsoleOutputService consoleOut)
        {
            this.consoleOut = consoleOut;

            this.Items = new List<InventoryItem>();
            this.Characters = new List<Character>();
            this.Features = new List<RoomFeature>();
            this.Visitors = new List<Character>();
        }

        public string Name
        {
            get;
            set;
        }

        public override bool TryEnter(Character character)
        {
            if (!this.Visitors.Contains(character))
            {
                this.Visitors.Add(character);

                var leader = character as ILeader;
                if (leader != null)
                {
                    foreach (var follower in leader.Followers)
                    {
                        this.TryEnter(follower);
                    }
                }
            }
            return true;
        }

        public override bool IsAccessible(Character character)
        {
            return true;
        }

        public override bool HasVisited(Character character)
        {
            return this.Visitors.Contains(character);
        }

        public List<InventoryItem> Items
        {
            get;
        }

        public List<Character> Characters
        {
            get;
        }

        public List<RoomFeature> Features
        {
            get;
        }

        public List<Character> Visitors
        {
            get;
        }

        public IEnumerable<TFeature> GetFeatures<TFeature>() where TFeature : RoomFeature
        {
            return from f in this.Features
                   where f.GetType().IsAssignableFrom(typeof(TFeature))
                   select (TFeature)f;
        }

        public Character GetCharacterByName(string characterName)
        {
            if (string.IsNullOrEmpty(characterName))
            {
                return null;
            }

            var character = (from c in this.Characters
                             where (c.Name.ToLower().CompareTo(characterName.ToLower()) == 0)
                             select c).FirstOrDefault();
            if (character == null)
            {
                character = (from c in this.Visitors
                             where (c.Name.ToLower().CompareTo(characterName.ToLower()) == 0)
                             select c).FirstOrDefault();
            }

            return character;
        }

        public IEnumerable<Enemy> GetEnemies()
        {
            return (from c in this.Characters
                    where typeof(Enemy).IsAssignableFrom(c.GetType())
                    select (Enemy)c).ToList();
        }

        public IEnumerable<Character> GetLiveCharacters()
        {
            return from c in this.Characters
                   where (c.IsDead() == false)
                   select c;
        }

        public IEnumerable<Character> GetDeadCharacters()
        {
            return from c in this.Characters
                   where (c.IsDead() == true)
                   select c;
        }

        public InventoryItem TakeItemByName(string itemName)
        {
            foreach (var curItem in this.Items)
            {
                if (curItem.Name.ToLower() == itemName)
                {
                    return curItem;
                }
            }

            foreach (var roomFeature in this.Features)
            {
                var featureItemProvider = roomFeature as IItemProvider;
                if (featureItemProvider != null)
                {
                    var item = featureItemProvider.TakeItemByName(itemName);
                    if (item != null)
                    {
                        return item;
                    }
                }
            }

            foreach (var roomCharacter in this.Characters)
            {
                var characterItemProvider = roomCharacter as IItemProvider;
                if (characterItemProvider != null)
                {
                    var item = characterItemProvider.TakeItemByName(itemName);
                    if (item != null)
                    {
                        return item;
                    }
                }
            }

            return null;
        }

        public void AddItem(InventoryItem item)
        {
            this.Items.Add(item);
        }

        public void RemoveItem(InventoryItem item)
        {
            this.Items.Remove(item);
        }

        public override void Display()
        {
            if (this.GetFeatures<Portal>().Count() > 0)
            {
                // This room has at least one portal. Display it
                // with a special character.
                this.consoleOut.Write("?");
            }
            else
            {
                this.consoleOut.Write(" ");
            }
        }

        public override void DisplayDescription()
        {
            this.consoleOut.WriteLine("");
            this.consoleOut.WriteLine(string.Format("You are in the {0} room", this.Name));

            bool isRoomEmpty = true;

            if (this.Features.Count > 0)
            {
                isRoomEmpty = false;

                this.consoleOut.WriteLine("");
                this.consoleOut.WriteLine("This room contains the following features:");

                foreach (var feature in this.Features)
                {
                    this.consoleOut.WriteLine(string.Format("   {0} - {1}", feature.Name, feature.Description));
                }
            }

            if (this.Items.Count > 0)
            {
                isRoomEmpty = false;

                this.consoleOut.WriteLine("");
                this.consoleOut.WriteLine("This room contains the following items:");

                foreach (var item in this.Items)
                {
                    this.consoleOut.WriteLine(string.Format("   {0} - {1}", item.Name, item.Description));
                }
            }

            if (this.Characters.Count > 0)
            {
                isRoomEmpty = false;

                var liveCharacters = this.GetLiveCharacters();
                var deadCharacters = this.GetDeadCharacters();

                if (liveCharacters.Count() > 0)
                {
                    this.consoleOut.WriteLine("");
                    this.consoleOut.WriteLine("This room contains the following characters:");

                    foreach (var character in liveCharacters)
                    {
                        this.consoleOut.WriteLine(string.Format("   {0} - {1}", character.Name, character.Description));
                    }
                }

                if (deadCharacters.Count() > 0)
                {
                    this.consoleOut.WriteLine("");
                    this.consoleOut.WriteLine("This room contains the following corpses:");

                    foreach (var character in deadCharacters)
                    {
                        this.consoleOut.WriteLine(string.Format("   {0} - {1}", character.Name, character.Description));
                    }
                }
            }

            if (isRoomEmpty)
            {
                this.consoleOut.WriteLine("Emptiness everywhere you look! Depressing, isn't it?");
            }
        }
    }
}
