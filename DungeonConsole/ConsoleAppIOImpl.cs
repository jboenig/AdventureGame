using System;

using AdventureGameEngine;

namespace DungeonConsole
{
    internal sealed class ConsoleAppIOImpl : IConsoleOutputService, IUserPromptService
    {
        public void Write(string msg)
        {
            Console.Write(msg);
        }

        public void Write(char c)
        {
            Console.Write(c);
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void WriteLine(string msg)
        {
            Console.WriteLine(msg);
        }

        public void Clear()
        {
            Console.Clear();
        }

        public bool PromptBool(string dlgTitle, string displayText)
        {
            this.Write(displayText);
            var answer = Console.ReadKey();
            if (answer.KeyChar == 'y' || answer.KeyChar == 'Y')
            {
                return true;
            }
            return false;
        }

        public string PromptText(string dlgTitle, string displayText)
        {
            this.Write(displayText);
            return Console.ReadLine();
        }
    }
}
