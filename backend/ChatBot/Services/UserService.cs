using ChatBot.Extensions;
using ChatBot.Models;
using ChatBot.Repositories.Interfaces;
using ChatBot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        return _httpClient.GetByIdAsync(id).Result;
    }
    
    public User? GetByEmail(string email)
    {
        return _httpClient.GetByEmailAsync(email).Result;
    }

    public ISet<User> GetAllUsers()
    {
        return _httpClient.GetAllAsync().Result;
    }

    public ISet<IParticipant> GetParticipantsByConversationID(Guid conversationID)
    {
        throw new NotImplementedException();
    }
}
