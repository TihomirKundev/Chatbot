using FakeAPI.Entities;

namespace Fake_API.DAL.Repository;

public interface IUserRepository
{
    User CheckAndGetUSer(string email, string password);
}