using ChatBot.Models;
using ChatBot.Models.DTOs;
using ChatBot.Models.Enums;
using System;

namespace ChatBot.Services.Interfaces
{
    public interface IConversationService
    {
        void AddMessageToConversation(MessageDTO messageDTO, Guid conversationID);
        void AddParticipantToConversation(IParticipant participant, Guid conversationID);
        Conversation CreateNewConversation(Guid id);
        Conversation? GetConversationById(Guid id);
        void SetConversationStatus(Guid conversationID, ConversationStatus status);
    }
}