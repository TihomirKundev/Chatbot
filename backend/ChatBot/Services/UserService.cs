using ChatBot.Extensions;
using ChatBot.Models;
using ChatBot.Repositories.Interfaces;
using ChatBot.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace ChatBot.Services;

[TransientService]
public class UserService : IUserService
{
    public User? GetById(Guid id)
    {
        throw new NotImplementedException();
    }
    
    public User? GetByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public ISet<User> GetAllUsers()
    {
        throw new NotImplementedException();
    }

    public ISet<IParticipant> GetParticipantsByConversationID(Guid conversationID)
    {
        throw new NotImplementedException();
    }
}
