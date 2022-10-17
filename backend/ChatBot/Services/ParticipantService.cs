using ChatBot.Extensions;
using ChatBot.Models;
using ChatBot.Repositories.Interfaces;
using ChatBot.Services.Interfaces;
using System;

namespace ChatBot.Services;

[TransientService]
public class ParticipantService : IParticipantService
{
    private readonly IUserRepository _userRepo;

    private readonly IAccountRepository _accountRepo;

    public ParticipantService(IUserRepository userRepository, IAccountRepository accountRepository)
    {
        _userRepo = userRepository;
        _accountRepo = accountRepository;
    }

    public IParticipant? GetParticipantById(Guid id)
    {
        var bot = new Bot();
        if (id == bot.ID)
            return bot;

        Account? account = _accountRepo.GetAccountByID(id);

        if (account is not null)
            return account;

        if (_userRepo.UserExists(id))
            return new User(id);

        return null;
    }
}
