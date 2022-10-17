using ChatBot.Models;
using ChatBot.Models.DTOs;

namespace ChatBot.Services.Interfaces
{
    public interface IMessageService
    {
        Message CreateMessage(MessageDTO dto);
        bool DeleteMessageById(long id);
    }
}