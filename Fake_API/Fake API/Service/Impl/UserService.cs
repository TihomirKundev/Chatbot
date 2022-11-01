using System.Dynamic;
using Fake_API.DAL.Repository;
using Fake_API.DTOs;
using Fake_API.Entities.DTO;
using Fake_API.Exception;
using FakeAPI.Entities;

namespace Fake_API.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository repository)
        {
            this._userRepository = repository;
        }

        public User GetUser(LoginCredentialsDTO loginCredentialsDto)
        {
            var user = _userRepository.CheckAndGetUSer(loginCredentialsDto.Email, loginCredentialsDto.Password);
            if (user is null)
                throw new UserNotFoundException("User not found");
            return user;
        }

        public User GetById(Guid id)
        {
            User? user = _userRepository.GetById(id);
            if (user is null)
                throw new UserNotFoundException("No user found");
            return user;
        }

        public User GetByEmail(string email)
        {
            User? user = _userRepository.GetByEmail(email);
            if (user is null)
                throw new UserNotFoundException("No user found");
            return user;
        }

        public List<User> GetAllUsers()
        {
            List<User> users = _userRepository.GetAll();
            if (users.Count == 0)
                users = new List<User>();
            return users;
        }
    }
}
