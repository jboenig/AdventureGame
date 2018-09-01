using System;
using System.Collections.Generic;

namespace AdventureGameEngine
{
    public interface ICommandProcessor
    {
        bool ExecuteCommand(string commandLine);
        IList<string> CommandHistory { get; }
        event EventHandler CommandExecuted;
    }
}
