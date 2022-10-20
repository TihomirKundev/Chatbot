using ChatBot.Models;
using System;
using System.Collections.Generic;

namespace ChatBot.Services.Interfaces
{
    public interface IAccountService
    {
        List<Account> GetAllAccounts();
        void SaveAccount(Account account);
        Account GetById(Guid id);
    }
}