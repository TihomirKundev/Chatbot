using ChatBot.Extensions;
using ChatBot.Models;
using ChatBot.Repositories.Interfaces;
using ChatBot.Services.Interfaces;
using System;
using System.Collections.Generic;
using ChatBot.Http;

namespace ChatBot.Services;

[TransientService]
public class UserService : IUserService
{
    private IFakeApiHttpClient _httpClient;
   
    public UserService(IFakeApiHttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
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
