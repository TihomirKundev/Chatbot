using System;
using System.Collections.Generic;
using ChatBot.Models;
using ChatBot.Models.DTOs;


namespace ChatBot.Auth.Repository;

public interface IUserRepository
{
    public User RegisterUser(RegisterRequest request);
    public User GetById(Guid id);
    public User GetByEmail(string email);
    public List<User> GetAll();
    public void Save(User user);
}