using ChatBot.Extensions;
using ChatBot.Models;
using ChatBot.Models.DTOs;
using ChatBot.Models.Enums;

using ChatBot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using ChatBot.Repositories;
using ChatBot.Repositories.EFC;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Services;

[SingletonService]
public class ConversationService : IConversationService
{
    private readonly DatabaseContext _DBcontext;
   // private readonly ConversationRepository _conversationRepo;
    private readonly IUserService _userService;
    private readonly IMessageService _messageService;

    public event MessageSentEventHandler? MessageSent;
    public event ConversationUpdatedEventHandler? ConversationUpdated;

    public ConversationService(DatabaseContext context, 
                               IMessageService mesageService,IUserService userService)
    {
        _DBcontext = context;
        _messageService = mesageService;
        _userService = userService;
    }

    public Conversation CreateNewConversation()
    {
        var conversation = new Conversation(Guid.NewGuid());
        _DBcontext.Conversations.Add(conversation);
        _DBcontext.SaveChanges();
        ConversationUpdated?.Invoke(conversation);
        return conversation;
    }

    public Conversation? GetConversationById(Guid id)
    {
        return _DBcontext.Conversations.FirstOrDefault(x => x.ID == id);
    }

    public void AddMessageToConversation(MessageDTO messageDTO, Guid conversationID)
    {
        messageDTO.ID = Guid.NewGuid();
        var message = _messageService.CreateMessage(messageDTO);
       var a = _DBcontext.Conversations.FirstOrDefault(x => x.ID == conversationID).AddMessage(message);
       try
       {
           _DBcontext.SaveChanges();
       }
       catch (DbUpdateConcurrencyException ex)
       {
           foreach (var entry in ex.Entries)
           {
               if (entry.Entity is Message)
               {
                   var proposedValues = entry.CurrentValues;
                   var databaseValues = entry.GetDatabaseValues();
                   foreach (var property in proposedValues.Properties)
                   {
                       var proposedValue = proposedValues[property];
                       var databaseValue = databaseValues?[property];
                       proposedValues[property] = proposedValue;
                   }

                   entry.OriginalValues.SetValues(proposedValues);
               }else
               {
                   throw new NotSupportedException(
                       "Don't know how to handle concurrency conflicts for "
                       + entry.Metadata.Name);
               }

           }

           _DBcontext.SaveChanges();
       }
               
               








       MessageSent?.Invoke(conversationID, messageDTO);
        
    }

    public void AddParticipantToConversation(Guid participantID, Guid conversationID)
    {
        _DBcontext.Conversations.FirstOrDefault(x => x.ID == conversationID).Participants.Add(_userService.GetById(participantID));
        _DBcontext.SaveChanges();
        if (GetConversationById(conversationID) is { } conversation)
            ConversationUpdated?.Invoke(conversation);
    }

    public void SetConversationStatus(Guid conversationID, ConversationStatus status)
    {
        _DBcontext.Conversations.FirstOrDefault(x => x.ID == conversationID).Status = status;
    }

    public IEnumerable<Conversation> GetConversationsByUser(User user)
    {
        return _DBcontext.Conversations.Where(x => x.Participants.Contains(user));
    }

    public IEnumerable<Conversation> GetConversations()
    {
        return _DBcontext.Conversations;
    }
}
