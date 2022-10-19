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
    private readonly IUserRepository _userRepo;

    public UserService(IUserRepository userRepository)
    {
        _userRepo = userRepository;
    }
    
    public User? CreateNewUser(Guid id)
    {
        if (id == Bot.GetChatBotID())
            return null;

        if (_userRepo.UserExists(id))
            return new User(id);

        var user = new User(id);
        _userRepo.CreateUser(user);
        return user;
    }
    
    // NOTE: This does NOT return users that have accounts
    // See AccountService.GetAllAccounts() for that (or ParticipantService.GetAllParticipants() for both)
    public List<User> GetAllUsers()
    {
        return _userRepo.GetAllUsers();
    }
}
