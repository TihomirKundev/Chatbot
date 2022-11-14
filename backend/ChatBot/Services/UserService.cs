using ChatBot.Extensions;
using ChatBot.Http;
using ChatBot.Models;
using ChatBot.Services.Interfaces;
using System;
using System.Collections.Generic;


namespace ChatBot.Services;

[TransientService]
public class UserService : IUserService
{
    private IUserHttpClient _httpClient;

    public UserService(IUserHttpClient httpClient)
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
    
    
}
