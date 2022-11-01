using ChatBot.Models;
using System;

namespace ChatBot.Auth.Jwt
{
    public interface IJwtUtils
    {
        string GenerateToken(User account);
        Guid? ValidateToken(string token);
    }
}