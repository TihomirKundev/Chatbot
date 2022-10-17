using ChatBot.Models;
using System;

namespace ChatBot.Services.Interfaces
{
    public interface IParticipantService
    {
        IParticipant? GetParticipantById(Guid id);
    }
}