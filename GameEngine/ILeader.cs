using System;
using System.Collections.Generic;

namespace AdventureGameEngine
{
    public interface ILeader
    {
        void Follow(Character character);
        IEnumerable<Character> Followers { get; }
        Character GetFollowerByName(string characterName);
    }
}
