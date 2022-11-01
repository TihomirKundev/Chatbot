using Fake_API.DTOs;
using FakeAPI.Entities;

namespace Fake_API.Service;

public interface IUserService
{
    User GetUser(LoginCredentialsDTO loginCredentialsDto);
}