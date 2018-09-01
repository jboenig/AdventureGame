using System;
using Ninject;
using AdventureGameEngine;

namespace Dungeon
{
    public class DungeonGame : Game
    {
        public DungeonGame(IGameHost gameHost,
            IConsoleOutputService consoleOut,
            IUserPromptService userPrompt,
            ISoundPlayerService soundPlayer) :
            base(gameHost, consoleOut, userPrompt, soundPlayer)
        {
        }

        public override GameBoard CreateGameBoard(string mapName)
        {
            return new DungeonBoard(this.ConsoleOut, this.SoundPlayer, this.UserPrompt, mapName);
        }

        public override Player CreatePlayer()
        {
            var player = new Player(this.ConsoleOut, this.SoundPlayer);
            player.Position = this.GameBoard.StartPosition;
            player.AddToInventory(new Flask(this.ConsoleOut, this.SoundPlayer));
            player.AddToInventory(new CoinPurse(this.ConsoleOut, this.SoundPlayer));
            player.AddToInventory(new Dagger(this.ConsoleOut, this.SoundPlayer));
            return player;
        }
    }
}
