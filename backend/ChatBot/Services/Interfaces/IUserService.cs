using ChatBot.Models;
using System;
using System.Collections.Generic;

namespace ChatBot.Services.Interfaces
{
    public interface IUserService
    {
        User? GetById(Guid id);

        User? GetByEmail(string email);

        ISet<User> GetAllUsers();
    }
}