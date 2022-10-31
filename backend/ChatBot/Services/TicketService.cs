using ChatBot.Extensions;
using ChatBot.Models.DTOs;
using ChatBot.Services.Interfaces;
using System;
using System.Collections.Generic;
using ChatBot.Repositories.Interfaces;

namespace ChatBot.Services;

[SingletonService]
public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;

    public TicketService(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public TicketDTO CreateTicket(TicketCreateDTO incomingTicket)
    {
        var ticket = new TicketDTO()
        {
            ticketnumber = Guid.NewGuid().ToString(), 
            name = incomingTicket.name,
            email = incomingTicket.email,
            status = Status.OPENED 
        };
     
        _ticketRepository.SaveTicket(ticket);
        return ticket;
    }

    public TicketDTO[] GetAllTickets() => _ticketRepository.GetAll();
    
}