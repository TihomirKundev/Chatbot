using ChatBot.Models.DTOs;
using ChatBot.Repositories.EFC;

namespace ChatBot.Services.Interfaces
{
    public interface ITicketService
    {
      
        TicketDTO[] GetAllTickets();
        TicketDTO CreateTicket(TicketCreateDTO incomingTicket);
    }
}