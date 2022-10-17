using ChatBot.Models;
using ChatBot.Models.Enums;
using System;

namespace ChatBot.Repositories.Interfaces
{
    public interface IConversationRepository
    {
        void SaveMessageToConversation(Message message, Guid conversationID);
        void CreateConversation(Conversation conversation);
        Conversation? GetConversationByID(Guid id);

        void SetConversationStatus(Guid id, ConversationStatus status);
        void AddParticipantToConversation(Guid participantID, Guid conversationID);
    }
}