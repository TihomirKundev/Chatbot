using Fake_API.DTOs;
using Fake_API.Entities.DTO;
using FakeAPI.Entities;

namespace Fake_API.Service;

public interface IUserService
{
    User GetUser(LoginCredentialsDTO loginCredentialsDto);
    User? GetById(Guid id);
    User? GetByEmail(string email);
    List<User> GetAllUsers();
    
}