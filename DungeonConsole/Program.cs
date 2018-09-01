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
            [Option('m', "map", Required = false,
              HelpText = "Name of map to use.")]
            public string MapName { get; set; }

            [ParserState]
            public IParserState LastParserState { get; set; }

            [HelpOption]
            public string GetUsage()
            {
                return HelpText.AutoBuild(this,
                  (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
            }
        }

        static void Main(string[] args)
        {
            var cmdLineOptions = new CommandLineOptions();
            CommandLine.Parser.Default.ParseArguments(args, cmdLineOptions);

            NInjectHelper.Initialize();

            var gameHost = NInjectHelper.Get<IGameHost>();
            var game = NInjectHelper.Get<Game>();
            game.MapName = cmdLineOptions.MapName;
            gameHost.Run(game);
        }
    }
}
