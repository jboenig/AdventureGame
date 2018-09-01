using System;
using Ninject;
using AdventureGameEngine;

namespace DungeonConsole
{
    public static class NInjectHelper
    {
        private static IKernel Kernel
        {
            get;
            set;
        }

        public static void Initialize()
        {
            Kernel = new StandardKernel();

            Kernel.Bind<Game>().To<Dungeon.DungeonGame>();
            Kernel.Bind<IGameHost>().ToConstant<ConsoleGameHost>(new ConsoleGameHost());
            Kernel.Bind<IConsoleOutputService>().To<ConsoleAppIOImpl>();
            Kernel.Bind<IUserPromptService>().To<ConsoleAppIOImpl>();
            Kernel.Bind<ISoundPlayerService>().To<SoundPlayerService>();
        }

        public static T Get<T>()
        {
            return Kernel.Get<T>();
        }

        public static void Inject(object obj, Ninject.Parameters.Parameter[] parameters)
        {
            Kernel.Inject(obj, parameters);
        }

        public static void Inject(object obj)
        {
            NInjectHelper.Inject(obj, null);
        }
    }
}
