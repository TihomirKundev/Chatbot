using ChatBot.Models;
using System;

namespace ChatBot.Auth.Jwt
{
    public interface IJwtUtils
    {
        string GenerateToken(Account account);
        Guid? ValidateToken(string token);
    }
}