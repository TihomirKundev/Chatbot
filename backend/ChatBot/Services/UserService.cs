using ChatBot.Extensions;
using ChatBot.Http;
using ChatBot.Models;
using ChatBot.Models.Enums;
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

    private readonly static User botUser = new User(Bot.GetChatBotID(), "Bot", "Bas World", "bot@basworld.com", "+31 413 72 83 20", "", Role.ADMIN);

    public User? GetById(Guid id)
    {
        if (id == Bot.GetChatBotID())
            return botUser;

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
