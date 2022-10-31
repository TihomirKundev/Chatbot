using Fake_API.DAL.Repository;
using Fake_API.DTOs;
using Fake_API.Exception;
using FakeAPI.DAL;
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
           var user =_userRepository.CheckAndGetUSer(loginCredentialsDto.Email,loginCredentialsDto.Password);
           if (user is null)
               throw new UserNotFoundException("User not found");
           return user;
        }
    }
}
