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

    /// <summary>
    /// This class encapsulates a conversation between multiple participants.
    /// Any object that implements <see cref="IConversationParticipant"/> can
    /// listen (hear) the conversation.
    /// </summary>
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

        /// <summary>
        /// Starts a new conversation with one participant.
        /// </summary>
        /// <param name="participant1">
        /// Participant in the conversation.
        /// </param>
        /// <returns>
        /// Returns a new <see cref="Conversation"/> object containing
        /// a single participant.
        /// </returns>
        public static Conversation Start(IConversationParticipant participant1)
        {
            var conversation = new Conversation();
            conversation.Join(participant1);
            return conversation;
        }

        /// <summary>
        /// Starts a new conversation with two participants.
        /// </summary>
        /// <param name="participant1">First participant</param>
        /// <param name="participant2">Second participant</param>
        /// <returns>
        /// Returns a new <see cref="Conversation"/> object containing
        /// the two participants.
        /// </returns>
        public static Conversation Start(IConversationParticipant participant1, IConversationParticipant participant2)
        {
            var conversation = new Conversation();
            conversation.Join(participant1);
            conversation.Join(participant2);
            return conversation;
        }

        /// <summary>
        /// Determines if the specified name is a participant in
        /// the conversation.
        /// </summary>
        /// <param name="participantName">Name of participant to check</param>
        /// <returns>
        /// Returns true if the conversation contains a participant with the
        /// specified name, otherwise returns false.
        /// </returns>
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

        /// <summary>
        /// Adds the specified participant to the conversation.
        /// </summary>
        /// <param name="participant">Participant to add</param>
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

        /// <summary>
        /// Removes the specified participant from the conversation.
        /// </summary>
        /// <param name="participantName">
        /// Name of participant to remove.
        /// </param>
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

        /// <summary>
        /// Broadcasts the specified message from the specified participant
        /// to all other participants in the conversation.
        /// </summary>
        /// <param name="participantName">
        /// Name of participant speaking.
        /// </param>
        /// <param name="dialogText">Message to send</param>
        public void Say(string participantName, string dialogText)
        {
            var source = (from p in this.participants
                          where p.Name == participantName
                          select p).FirstOrDefault();

            if (source == null)
            {
                var msg = string.Format("{0} is not a participant in this conversation", participantName);
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
