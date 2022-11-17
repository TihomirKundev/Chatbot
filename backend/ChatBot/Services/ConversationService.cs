using ChatBot.Extensions;
using ChatBot.Models;
using ChatBot.Models.DTOs;
using ChatBot.Models.Enums;
using ChatBot.Repositories.Interfaces;
using ChatBot.Services.Interfaces;
using System;
using System.Linq;
using ChatBot.Repositories;
using ChatBot.Repositories.EFC;

namespace ChatBot.Services;

[TransientService]
public class ConversationService : IConversationService 
{
    private readonly DatabaseContext _DBcontext;
    private readonly ConversationRepository _conversationRepo;
    private readonly IMessageService _messageService;
    private readonly IUserService _userService;
    
    public ConversationService(DatabaseContext context,
                               IMessageService mesageService,IUserService userService)
    {
        _DBcontext = context;
        _messageService = mesageService;
        _userService = userService;
    }

    public Conversation CreateNewConversation(Guid id)
    {
        var conversation = new Conversation(id);
        _DBcontext.Chats.Add(conversation);
        _DBcontext.SaveChanges();
        return conversation;
    }

    public Conversation? GetConversationById(Guid id)
    {
        return _DBcontext.Chats.FirstOrDefault(x => x.ID == id);
    }

    public void AddMessageToConversation(MessageDTO messageDTO, Guid conversationID)
    {
        var message = _messageService.CreateMessage(messageDTO);
        _DBcontext.Chats.FirstOrDefault(x => x.ID == conversationID).Messages.Add(message);
        _DBcontext.SaveChanges();
    }

    public void AddParticipantToConversation(Guid participantID, Guid conversationID)
    {
        _conversationRepo.AddParticipantToConversation(participantID, conversationID);
        _DBcontext.Chats.FirstOrDefault(x => x.ID == conversationID).Participants.Add(_userService.GetById(participantID));
        _DBcontext.SaveChanges();
    }

    public void SetConversationStatus(Guid conversationID, ConversationStatus status)
    {
       _DBcontext.Chats.FirstOrDefault(x => x.ID == conversationID).Status = status;
    }
}
