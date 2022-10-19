using ChatBot.Models;
using System;
using System.Collections.Generic;

namespace ChatBot.Repositories.Interfaces
{
    public interface IParticipantRepository
    {
        IParticipant? GetParticipantByID(Guid id);
        ISet<IParticipant> GetParticipantsByConversationID(Guid id);
    }
}