using ChatBot.Models;
using System;
using System.Collections.Generic;

namespace ChatBot.Services.Interfaces
{
    public interface IUserService
    {
        User? CreateNewUser(Guid id);
        List<User> GetAllUsers();
    }
}