using ChatBot.Models;
using System;
using System.Collections.Generic;

namespace ChatBot.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Account? GetAccountByID(Guid id);
        List<Account> GetAllAccounts();
        void SaveAccount(Account account);
    }
}