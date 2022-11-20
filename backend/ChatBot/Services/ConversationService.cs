using ChatBot.Extensions;
using ChatBot.Models;
using ChatBot.Models.DTOs;
using ChatBot.Models.Enums;
using ChatBot.Repositories.Interfaces;
using ChatBot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatBot.Services;

[SingletonService]
public class ConversationService : IConversationService
{
    private readonly IConversationRepository _conversationRepo;

    private readonly IMessageService _messageService;

    public event MessageSentEventHandler? MessageSent;
    public event ConversationUpdatedEventHandler? ConversationUpdated;

    public ConversationService(IConversationRepository conversationRepo,
                               IMessageService mesageService)
    {
        _conversationRepo = conversationRepo;
        _messageService = mesageService;
    }

    public Conversation CreateNewConversation()
    {
        var conversation = new Conversation(Guid.NewGuid());
        _conversationRepo.CreateConversation(conversation);
        ConversationUpdated?.Invoke(conversation);
        return conversation;
    }

    public Conversation? GetConversationById(Guid id)
    {
        return _conversationRepo.GetConversationByID(id);
    }

    public void AddMessageToConversation(MessageDTO messageDTO, Guid conversationID)
    {
        messageDTO.ID = Guid.NewGuid();
        var message = _messageService.CreateMessage(messageDTO);
        _conversationRepo.SaveMessageToConversation(message, conversationID);
        MessageSent?.Invoke(conversationID, messageDTO);
    }

    public void AddParticipantToConversation(Guid participantID, Guid conversationID)
    {
        _conversationRepo.AddParticipantToConversation(participantID, conversationID);
        if (GetConversationById(conversationID) is { } conversation)
            ConversationUpdated?.Invoke(conversation);
    }

    public void SetConversationStatus(Guid conversationID, ConversationStatus status)
    {
        _conversationRepo.SetConversationStatus(conversationID, status);
    }

    public IEnumerable<Conversation> GetConversationsByUser(User user)
    {
        return _conversationRepo.GetConversationsByUser(user);
    }

    public IEnumerable<Conversation> GetConversations()
    {
        return _conversationRepo.GetConversations();
    }
}
