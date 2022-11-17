using System;
using ChatBot.Models.DTOs;
using ChatBot.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ChatBot.Repositories;

//wrapper class that calls existing methods, no extra behaviour
public class TicketCachedRepository : ITicketRepository
{
    private readonly TicketRepository _decorated; //added decorated dependency 
    private readonly IMemoryCache _cache;
    private const string KEY = "cacheKey";

    public TicketCachedRepository(TicketRepository decorated, IMemoryCache cache)
    {
        _decorated = decorated;
        _cache = cache;
    }

    public void SaveTicket(TicketDTO ticket)
    {
        _decorated.SaveTicket(ticket);

    }

    public TicketDTO[] GetAll()
    {
        return _cache.GetOrCreate(KEY, entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
            return _decorated.GetAll();
        });
    }
}