using System;

namespace AdventureGameEngine
{
    public interface IMovePlayer
    {
        BoolMessageResult MoveTo(GameBoard.Position pos);
    }
}
