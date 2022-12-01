using MockAPI.Exceptions;
using MockAPI.DAL.Repository;
using MockAPI.DTOs;
using MockAPI.Entities;

namespace MockAPI.Service.Impl
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository repository)
        {
            _userRepository = repository;
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
            var user = _userRepository.GetById(id);
            if (user is null)
                throw new UserNotFoundException("No user found");
            return user;
        }

        public User GetByEmail(string email)
        {
            var user = _userRepository.GetByEmail(email);
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
