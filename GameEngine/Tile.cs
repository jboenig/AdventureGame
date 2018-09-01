using System;

namespace AdventureGameEngine
{
    /// <summary>
    /// This class represents a location on the game board.
    /// </summary>
    public abstract class Tile
    {
        public abstract void Display();
        public abstract void DisplayDescription();
        public abstract bool TryEnter(Character character);
        public abstract bool IsAccessible(Character character);
        public abstract bool HasVisited(Character character);
    }
}
