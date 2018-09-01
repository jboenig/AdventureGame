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
        public abstract bool CanExit(Character character);
        public abstract void Exit(Character character);
        public abstract bool CanEnter(Character character);
        public abstract void Enter(Character character);
        public abstract bool IsAccessible(Character character);
        public abstract bool HasVisited(Character character);
    }
}
