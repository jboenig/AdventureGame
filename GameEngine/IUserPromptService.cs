using System;

namespace AdventureGameEngine
{
    public interface IUserPromptService
    {
        bool PromptBool(string dlgTitle, string displayText);
        string PromptText(string dlgTitle, string displayText);
    }
}
