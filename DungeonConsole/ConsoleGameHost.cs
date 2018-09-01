using System;
using System.Linq;
using System.Text;

using AdventureGameEngine;

namespace DungeonConsole
{
    internal sealed class ConsoleGameHost : IGameHost
    {
        private Game game;
        private bool isGameRunning = false;
        private ConsoleAppIOImpl consoleIO;
        private SoundPlayerService soundPlayer;

        public ConsoleGameHost()
        {
            this.consoleIO = new ConsoleAppIOImpl();
            this.soundPlayer = new SoundPlayerService();
        }

        private Game Game
        {
            get { return this.game; }
        }

        private Player Player
        {
            get { return this.game.Player; }
        }

        #region IGameHost Interface

        /// <summary>
        /// This method runs the game.
        /// </summary>
        /// <param name="game"></param>
        public void Run(Game game)
        {
            this.game = game;
            this.isGameRunning = true;

            this.consoleIO.WriteLine("=========================================================");
            this.consoleIO.WriteLine("==                WELCOME TO THE DUNGEON               ==");
            this.consoleIO.WriteLine("=========================================================");

            this.Game.DisplayGameIntro();
            this.Game.DisplayCurrentRoomDescription();

            // Continue loop until flag is set to false

            while (this.isGameRunning)
            {
                this.consoleIO.WriteLine("");

                if (this.Player.IsDead())
                {
                    this.consoleIO.WriteLine("one ticket to a r movie pls - play again (Y/N)?");
                    var res = Console.ReadKey();
                    if (res.KeyChar == 'Y' || res.KeyChar == 'y')
                    {
                        this.Reset();
                    }
                    else
                    {
                        this.isGameRunning = false;
                    }
                }
                else if (this.Player.Position.IsUndefined)
                {
                    this.isGameRunning = false;
                    this.consoleIO.WriteLine("Congratulations, you've escaped! Play again?");
                    var res = Console.ReadKey();
                    if (res.KeyChar == 'Y' || res.KeyChar == 'y')
                    {
                        this.Reset();
                    }
                }
                else
                {
                    this.Game.DisplayCommandSeparator();
                    this.consoleIO.WriteLine("What would you like to do?");
                    var commandLine = this.ReadCommandLine();
                    if (!this.Game.ExecuteCommand(commandLine))
                    {
                        // Command line is invalid
                        this.consoleIO.WriteLine("That is not a valid command - type help for a list of commands");
                    }
                }
            }
        }

        public void Reset()
        {
            this.Game.Reset();
            this.isGameRunning = true;
            this.Game.DisplayGameIntro();
        }

        public void Exit()
        {
            this.isGameRunning = false;
        }

        #endregion

        public string ReadCommandLine()
        {
#if false
            // This is the easy way to do it, but
            // it doesn't give us the ability to
            // use up arrow to recall command history.
            var cmdLine = GlobalServices.Instance.ConsoleIO.ReadLine();
#else
            var cmdLineBuilder = new StringBuilder();
            int historyIdx = this.game.CommandHistory.Count();

            // Show prompt
            this.RefreshCommandLine("");

            var keyInfo = Console.ReadKey(true);

            while (keyInfo.Key != ConsoleKey.Enter)
            {
                if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (cmdLineBuilder.Length > 0)
                    {
                        cmdLineBuilder.Remove(cmdLineBuilder.Length - 1, 1);
                        this.RefreshCommandLine(cmdLineBuilder.ToString());
                    }
                }
                else if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    if (historyIdx > 0)
                    {
                        historyIdx = historyIdx - 1;
                        cmdLineBuilder.Clear();
                        cmdLineBuilder.Append(this.Game.CommandHistory[historyIdx]);
                        this.RefreshCommandLine(cmdLineBuilder.ToString());
                    }
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    if (historyIdx < this.Game.CommandHistory.Count() - 1)
                    {
                        historyIdx = historyIdx + 1;
                    }
                    if (historyIdx >= this.Game.CommandHistory.Count())
                    {
                        historyIdx = this.Game.CommandHistory.Count() - 1;
                    }
                    cmdLineBuilder.Clear();
                    cmdLineBuilder.Append(this.Game.CommandHistory[historyIdx]);
                    this.RefreshCommandLine(cmdLineBuilder.ToString());
                }
                else
                {
                    // Echo the key out to the screen
                    Console.Write(keyInfo.KeyChar);
                    // Append the key to our command line builder
                    cmdLineBuilder.Append(keyInfo.KeyChar);
                }

                keyInfo = Console.ReadKey(true);
            }

            Console.WriteLine();

            var cmdLine = cmdLineBuilder.ToString();
#endif
            return cmdLine;
        }

        private void RefreshCommandLine(string cmdLineText)
        {
            // This seems to work and is more precise
            while (Console.CursorLeft > 1)
            {
                Console.CursorLeft = Console.CursorLeft - 1;
                Console.Write(' ');
                Console.CursorLeft = Console.CursorLeft - 1;
            }
            Console.CursorLeft = 0;
            Console.CursorLeft = 0;
            Console.Write("> ");
            Console.Write(cmdLineText);
        }
    }
}
