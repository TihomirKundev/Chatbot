using ChatBot.Extensions;
using ChatBot.Models.DTOs;
using ChatBot.Repositories.Interfaces;
using ChatBot.Services.Interfaces;
using System;
using System.Linq;
using ChatBot.Repositories.EFC;


namespace ChatBot.Services;

[ScopedService]
public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;

    private readonly DatabaseContext _context;
    
    
    public TicketService(ITicketRepository ticketRepository, DatabaseContext context)
    {
        _ticketRepository = ticketRepository;
        _context = context;
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
        _context.Participants.Add(ticket);
      //  _ticketRepository.SaveTicket(ticket);
      _context.SaveChanges();
        return ticket;
    }



    public TicketDTO[] GetAllTickets() => _context.Participants.ToArray();

}