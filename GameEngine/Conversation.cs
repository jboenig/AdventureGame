using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventureGameEngine
{
    public interface IConversationParticipant
    {
        string Name { get; }
        void Hear(Conversation conversation, IConversationParticipant source, string dialogText);
    }

    public sealed class Conversation
    {
        private List<IConversationParticipant> participants;

        private Conversation()
        {
            this.participants = new List<IConversationParticipant>();
        }

        public static Conversation Start()
        {
            return new Conversation();
        }

        public static Conversation Start(IConversationParticipant participant1)
        {
            var conversation = new Conversation();
            conversation.Join(participant1);
            return conversation;
        }

        public static Conversation Start(IConversationParticipant participant1, IConversationParticipant participant2)
        {
            var conversation = new Conversation();
            conversation.Join(participant1);
            conversation.Join(participant2);
            return conversation;
        }

        public bool IsParticipant(string participantName)
        {
            if (string.IsNullOrEmpty(participantName))
            {
                return false;
            }

            var existingParticipant = (from p in this.participants
                                       where p.Name == participantName
                                       select p).FirstOrDefault();
            return (existingParticipant != null);
        }

        public void Join(IConversationParticipant participant)
        {
            if (participant == null)
            {
                throw new ArgumentNullException("participant");
            }
            var existingParticipant = (from p in this.participants
                                       where p.Name == participant.Name
                                       select p).FirstOrDefault();

            if (existingParticipant != null)
            {
                var msg = string.Format("{0} is already participating in this conversation", participant.Name);
                throw new ArgumentNullException(msg);
            }

            this.participants.Add(participant);
        }

        public void Leave(string participantName)
        {
            var existingParticipant = (from p in this.participants
                                       where p.Name == participantName
                                       select p).FirstOrDefault();

            if (existingParticipant != null)
            {
                this.participants.Remove(existingParticipant);
            }
        }

        public void Say(string sourceName, string dialogText)
        {
            var source = (from p in this.participants
                          where p.Name == sourceName
                          select p).FirstOrDefault();

            if (source == null)
            {
                var msg = string.Format("{0} is not a participant in this conversation", sourceName);
                throw new ArgumentException(msg, "participantName");
            }

            foreach (var participant in this.participants)
            {
                if (participant != source)
                {
                    participant.Hear(this, source, dialogText);
                }
            }
        }
    }
}
