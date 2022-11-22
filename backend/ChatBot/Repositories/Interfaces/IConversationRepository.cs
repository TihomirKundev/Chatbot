using ChatBot.Models;
using ChatBot.Models.Enums;
using System;
using System.Collections.Generic;

namespace ChatBot.Repositories.Interfaces
{
    public interface IConversationRepository
    {
        void SaveMessageToConversation(Message message, Guid conversationID);
        void CreateConversation(Conversation conversation);
        Conversation? GetConversationByID(Guid id);
        void SetConversationStatus(Guid id, ConversationStatus status);
        void AddParticipantToConversation(Guid participantID, Guid conversationID);
        ISet<IParticipant> GetParticipantsByConversationID(Guid id);
        SortedSet<Message> GetAllMessagesByConversationID(Guid conversationID);
        bool DeleteMessageById(long id);
        IEnumerable<Conversation> GetConversationsByUser(User user);
        IEnumerable<Conversation> GetConversations();
    }
}