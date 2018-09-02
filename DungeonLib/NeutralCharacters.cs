using System;
using System.Collections.Generic;
using AdventureGameEngine;

namespace Dungeon
{
    public class Wizard : NeutralCharacter, IConversationParticipant
    {
        private readonly List<string> runeClues = new List<string>();

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

            if (dialogText.Contains("help"))
            {
                if (dialogText.Contains("rune"))
                {
                    this.GiveClue(conversation);
                }
                else
                {
                    conversation.Say(this.Name, "Help will be given to those who help themselves");
                }
            }
            else
            {
                conversation.Say(this.Name, "Read the runes to discover the password to the exit portal");
            }
        }

        public void ReadRunes(RuneCollection runes)
        {
            foreach (var r in runes)
            {
                this.runeClues.Add(r.Text);
            }
        }

        private void GiveClue(Conversation conversation)
        {
            if (this.runeClues.Count > 0)
            {
                var runeIdx = DateTime.Now.Second % this.runeClues.Count;
                if (runeIdx < 0 || runeIdx >= this.runeClues.Count)
                {
                    throw new InvalidOperationException();
                }
                conversation.Say(this.Name, this.runeClues[runeIdx]);
            }
        }
    }
}
