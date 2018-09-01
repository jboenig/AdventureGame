namespace AdventureGameEngine
{
    /// <summary>
    /// Interface to object that hosts the game engine.
    /// </summary>
    public interface IGameHost
    {
        /// <summary>
        /// Runs the specified game.
        /// </summary>
        /// <param name="game">
        /// Reference to game to run.
        /// </param>
        void Run(Game game);

        /// <summary>
        /// Resets the current game.
        /// </summary>
        void Reset();

        /// <summary>
        /// Exits the currently running game.
        /// </summary>
        void Exit();
    }
}
