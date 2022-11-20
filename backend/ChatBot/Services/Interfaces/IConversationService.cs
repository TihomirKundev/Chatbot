using ChatBot.Models;
using ChatBot.Models.DTOs;
using ChatBot.Models.Enums;
using System;
using System.Collections.Generic;

namespace ChatBot.Services.Interfaces
{
    public delegate void MessageSentEventHandler(Guid conversationID, MessageDTO message);
    public delegate void ConversationUpdatedEventHandler(Conversation conversation);

    public interface IConversationService
    {
        event MessageSentEventHandler? MessageSent;
        event ConversationUpdatedEventHandler? ConversationUpdated;

        void AddMessageToConversation(MessageDTO messageDTO, Guid conversationID);
        void AddParticipantToConversation(Guid participantID, Guid conversationID);
        Conversation CreateNewConversation();
        Conversation? GetConversationById(Guid id);
        void SetConversationStatus(Guid conversationID, ConversationStatus status);
        IEnumerable<Conversation> GetConversationsByUser(User user);
        IEnumerable<Conversation> GetConversations();
    }
}