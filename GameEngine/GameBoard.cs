using System;

namespace AdventureGameEngine
{
    /// <summary>
    /// This class represents the entire game board.
    /// </summary>
    public abstract class GameBoard
    {
        private int rowCount;
        private int columnCount;

        /// <summary>
        /// Two-dimensional array of map entries.
        /// </summary>
        private Tile[,] tiles;

        public GameBoard()
        {
        }

        public GameBoard(int rowCount, int colCount)
        {
            this.RowCount = rowCount;
            this.ColumnCount = colCount;
        }

        public virtual void Initialize()
        {
            if (this.RowCount == 0 || this.ColumnCount == 0)
            {
                throw new InvalidOperationException("Dimensions of the GameBoard must be set before initialization");
            }
            this.tiles = new Tile[this.RowCount, this.ColumnCount];
        }

        public Tile[,] Tiles
        {
            get { return this.tiles; }
        }

        public int RowCount
        {
            get { return this.rowCount; }
            set { this.rowCount = value; }
        }

        public int ColumnCount
        {
            get { return this.columnCount; }
            set { this.columnCount = value; }
        }

        public abstract Position StartPosition
        {
            get;
        }

        public Tile GetEntry(Position pos)
        {
            if (pos.row < this.tiles.GetLength(0) &&
                pos.col < this.tiles.GetLength(1))
            {
                return this.tiles[pos.row, pos.col];
            }
            return null;
        }

        public bool IsValidPosition(Position pos)
        {
            return (pos.col >= 0 && pos.col < this.ColumnCount &&
                pos.row >= 0 && pos.row < this.RowCount);
        }

        /// <summary>
        /// Applies an action to each <see cref="GameBoard.Position"/>
        /// on the <see cref="GameBoard"/>.
        /// </summary>
        /// <param name="actor">
        /// Action to apply.
        /// </param>
        public void ForeachPosition(Action<GameBoard.Position> actor)
        {
            for (int row = 0; row < this.RowCount; row++)
            {
                for (int col = 0; col < this.ColumnCount; col++)
                {
                    var pos = new GameBoard.Position(row, col);
                    actor(pos);
                }
            }
        }

        /// <summary>
        /// Represents a position on a <see cref="GameBoard"/>.
        /// </summary>
        public sealed class Position
        {
            internal int row;
            internal int col;

            internal Position(int row, int col)
            {
                this.row = row;
                this.col = col;
            }

            public static Position Create(int row, int col)
            {
                return new Position(row, col);
            }

            public bool IsUndefined
            {
                get { return (this.col < 0 || this.row < 0); }
            }

            public static Position Undefined
            {
                get { return new Position(-1, -1); }
            }

            public Position Forward()
            {
                return new Position(this.row - 1, this.col);
            }

            public Position Back()
            {
                return new Position(this.row + 1, this.col);
            }

            public Position Left()
            {
                return new Position(this.row, this.col - 1);
            }

            public Position Right()
            {
                return new Position(this.row, this.col + 1);
            }

            public int Row
            {
                get { return this.row; }
            }

            public int Column
            {
                get { return this.col; }
            }

            public override string ToString()
            {
                return string.Format("Position = {0},{1}", this.row, this.col);
            }
        }

        public abstract void Display(Position playerPos);
    }
}
