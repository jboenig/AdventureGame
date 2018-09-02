using System;
using AdventureGameEngine;

namespace Dungeon
{
    public class Wizard : NeutralCharacter, IConversationParticipant
    {
        public Wizard(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer, string name, string desc) :
            base(consoleOut, soundPlayer, name, desc)
        {
        }

        public void Hear(Conversation conversation, IConversationParticipant source, string dialogText)
        {
            if (!(source is Player))
            {
                return;
            }

            conversation.Say(this.Name, "I am a wizard");
        }
    }
}
