using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatBot.Models;
using ChatBot.Models.DTOs;
using ChatBot.Models.Request;

namespace ChatBot.Http;

public interface IUserHttpClient
{
    Task<User> GetUserAsync(AuthenticateRequest request);
    Task<ISet<User>> GetAllAsync();
    Task<User> GetByEmailAsync(string email);
    Task<User> GetByIdAsync(Guid id);
    Task<FakeApiUserDTO> GetFakeApiUserDTOByIdAsync(Guid id);
}