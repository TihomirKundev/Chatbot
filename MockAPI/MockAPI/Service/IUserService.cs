using MockAPI.DTOs;
using MockAPI.Entities;

namespace MockAPI.Service;

public interface IUserService
{
    User GetUser(LoginCredentialsDTO loginCredentialsDto);
    User? GetById(Guid id);
    User? GetByEmail(string email);
    List<User> GetAllUsers();

}