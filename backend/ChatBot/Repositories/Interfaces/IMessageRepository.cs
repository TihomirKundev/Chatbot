using ChatBot.Models;
using System;
using System.Collections.Generic;

namespace ChatBot.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        SortedSet<Message> GetAllMessagesByConversationID(Guid conversationID);
        bool DeleteMessageById(long id);
    }
}