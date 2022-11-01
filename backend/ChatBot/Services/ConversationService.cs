using ChatBot.Extensions;
using ChatBot.Models;
using ChatBot.Models.DTOs;
using ChatBot.Models.Enums;
using ChatBot.Repositories.Interfaces;
using ChatBot.Services.Interfaces;
using System;
namespace ChatBot.Services;

[TransientService]
public class ConversationService : IConversationService
{
    private readonly IConversationRepository _conversationRepo;

    private readonly IMessageService _messageService;

    public ConversationService(IConversationRepository conversationRepo,
                               IMessageService mesageService)
    {
        _conversationRepo = conversationRepo;
        _messageService = mesageService;
    }

    public Conversation CreateNewConversation(Guid id)
    {
        var conversation = new Conversation(id);
        _conversationRepo.CreateConversation(conversation);
        return conversation;
    }

    public Conversation? GetConversationById(Guid id)
    {
        return _conversationRepo.GetConversationByID(id);
    }

    public void AddMessageToConversation(MessageDTO messageDTO, Guid conversationID)
    {
        var message = _messageService.CreateMessage(messageDTO);
        _conversationRepo.SaveMessageToConversation(message, conversationID);
    }

    public void AddParticipantToConversation(Guid participantID, Guid conversationID)
    {
        _conversationRepo.AddParticipantToConversation(participantID, conversationID);
    }

    public void SetConversationStatus(Guid conversationID, ConversationStatus status)
    {
        _conversationRepo.SetConversationStatus(conversationID, status);
    }


}
