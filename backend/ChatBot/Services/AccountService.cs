using ChatBot.Extensions;
using ChatBot.Models;
using ChatBot.Repositories.Interfaces;
using ChatBot.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace ChatBot.Services;

[TransientService]
public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepo;

    public AccountService(IAccountRepository accountRepo)
    {
        _accountRepo = accountRepo;

    }
    public void SaveAccount(Account account)
    {
        _accountRepo.SaveAccount(account);
    }

    public Account GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public List<Account> GetAllAccounts()
    {
        return _accountRepo.GetAllAccounts();
    }
    
    
}
