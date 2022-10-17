using ChatBot.Models;
using System;
using System.Collections.Generic;

namespace ChatBot.Repositories.Interfaces
{
    public interface IUserRepository
    {
        void CreateUser(User user);
        List<User> GetAllUsers();
        bool UserExists(Guid id);
    }
}