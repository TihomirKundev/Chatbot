using ChatBot.Models.DTOs;

namespace ChatBot.Services.Interfaces
{
    public interface ITicketService
    {
        TicketDTO CreateTicket(TicketCreateDTO incomingTicket);
        TicketDTO[] GetAllTickets();
    }
}