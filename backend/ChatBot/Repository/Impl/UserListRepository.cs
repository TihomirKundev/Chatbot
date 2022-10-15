using System;
using System.Collections.Generic;
using ChatBot.Models;
using ChatBot.Models.DTOs;

namespace ChatBot.Auth.Repository;

public class UserListRepository : IUserRepository
{
    private readonly List<User> allUsers = new();

    public User RegisterUser(RegisterRequest request)
    {
        Guid userId = Guid.NewGuid();
        Enum.TryParse(request.Role, out Role role);
        User user = new User(userId,
            request.FirstName,
            request.LastName,
            request.Password, 
            request.Email,
            request.Phone,
            role);
        allUsers.Add(user);
        return user;
    }
    
    public User GetById(Guid id) => allUsers.Find(user => user.ID == id);
    
    public User GetByEmail(string email) => allUsers.Find(user => user.Email == email);

    public List<User> GetAll() => allUsers;

    public void Save(User user) => allUsers.Add(user);

}