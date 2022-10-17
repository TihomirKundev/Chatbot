using ChatBot.Extensions;
using ChatBot.Models.DTOs;
using ChatBot.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace ChatBot.Services;

[SingletonService]
public class TicketService : ITicketService
{
    private List<TicketDTO> ticketList = new();

    public TicketService()
    {

    }

    public TicketDTO CreateTicket(TicketCreateDTO incomingTicket)
    {
        var ticket = new TicketDTO();
        ticket.ticketNumber = Guid.NewGuid().ToString();
        ticket.email = incomingTicket.email;
        ticket.name = incomingTicket.name;
        ticket.status = Status.OPENED;
        ticketList.Add(ticket);
        return ticket;
    }

    public TicketDTO[] GetAllTickets()
    {
        return ticketList.ToArray();
    }
}