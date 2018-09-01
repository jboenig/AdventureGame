using System;

namespace AdventureGameEngine
{
    /// <summary>
    /// This class represents a wall.
    /// </summary>
    public class Wall : Tile
    {
        private IConsoleOutputService consoleOut;

        public Wall(IConsoleOutputService consoleOut)
        {
            this.consoleOut = consoleOut;
        }

        public override void Display()
        {
            this.consoleOut.Write("X");
        }

        public override void DisplayDescription()
        {
            this.consoleOut.Write("Wall");
        }

        public override bool TryEnter(Character character)
        {
            return false;
        }

        public override bool IsAccessible(Character character)
        {
            return false;
        }

        public override bool HasVisited(Character character)
        {
            return false;
        }
    }
}
