using System;
using System.Collections.Generic;
using ChatBot.Models;
using ChatBot.Models.Enums;
using ChatBot.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ChatBot.Repositories;

public class ConversationCachedRepository : IConversationRepository
{
    private readonly ConversationRepository _decorated;
    private readonly IMemoryCache _cache;
    private const string KEY = "cacheKey";

    public ConversationCachedRepository(IMemoryCache cache, ConversationRepository decorated)
    {
        _cache = cache;
        _decorated = decorated;
    }

    public void SaveMessageToConversation(Message message, Guid conversationID)
    {
        throw new NotImplementedException();
    }

    public void CreateConversation(Conversation conversation)
    {
        throw new NotImplementedException();
    }

    public Conversation GetConversationByID(Guid id)
    {
        return _cache.GetOrCreate(KEY, entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
            return _decorated.GetConversationByID(id);
        });
    }

    public void SetConversationStatus(Guid id, ConversationStatus status)
    {
        throw new NotImplementedException();

    }

    public void AddParticipantToConversation(Guid participantID, Guid conversationID)
    {
        throw new NotImplementedException();
    }

    public ISet<Participant> GetParticipantsByConversationID(Guid id)
    {
        throw new NotImplementedException();
    }

    public SortedSet<Message> GetAllMessagesByConversationID(Guid conversationID)
    {
        return _cache.GetOrCreate(KEY, entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
            return _decorated.GetAllMessagesByConversationID(conversationID);
        });
    }

    public bool DeleteMessageById(long id)
    {
        throw new NotImplementedException();
    }
}