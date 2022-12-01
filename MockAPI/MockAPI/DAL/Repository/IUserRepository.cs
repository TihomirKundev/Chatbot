using MockAPI.Entities;

namespace MockAPI.DAL.Repository;

public interface IUserRepository
{
    User CheckAndGetUSer(string email, string password);
    User? GetByEmail(string email);
    User? GetById(Guid id);
    List<User> GetAll();
}