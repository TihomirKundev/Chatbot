using System.Collections.Generic;
using ChatBot.Models.DTOs;

namespace ChatBot.Repositories.Interfaces;

public interface ITicketRepository
{
    void SaveTicket(TicketDTO ticket);
    TicketDTO[] GetAll();
}