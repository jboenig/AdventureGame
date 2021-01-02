using System;
using CommandLine;
using CommandLine.Text;

using AdventureGameEngine;
using Dungeon;

namespace DungeonConsole
{
    class Program
    {
        class CommandLineOptions
        {
            [Option('m', "map", Required = false, HelpText = "Name of map to use.")]
            public string MapName { get; set; }
        }

        static void Main(string[] args)
        {
            var parseRes = Parser.Default.ParseArguments<CommandLineOptions>(args);
            parseRes.WithParsed((options) =>
            {
                NInjectHelper.Initialize();

                var gameHost = NInjectHelper.Get<IGameHost>();
                var game = NInjectHelper.Get<Game>();
                game.MapName = options.MapName;
                gameHost.Run(game);
            });
        }
    }
}
