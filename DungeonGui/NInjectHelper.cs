using System;
using Ninject;
using AdventureGameEngine;

namespace AdventureGameGui
{
    public static class NInjectHelper
    {
        private static IKernel Kernel
        {
            get;
            set;
        }

        public static void Initialize(MainWindow mainWindow)
        {
            if (mainWindow == null)
            {
                throw new ArgumentNullException("mainWindow");
            }

            Kernel = new StandardKernel();

            Kernel.Bind<Game>().To<Dungeon.DungeonGame>();
            Kernel.Bind<IGameHost>().ToConstant<IGameHost>(mainWindow);
            Kernel.Bind<IConsoleOutputService>().ToConstant<IConsoleOutputService>(mainWindow.consoleIO);
            Kernel.Bind<IUserPromptService>().ToConstant<IUserPromptService>(mainWindow);
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
