using Fake_API.DTOs;
using FakeAPI.DAL;
using FakeAPI.Entities;

namespace Fake_API.Service
{
    public class UserService
    {
        DALUser dalUser;

        public UserService()
        {
            this.dalUser = new DALUser();
        }
        public User GetUser(LoginCredentialsDTO loginCredentialsDto)
        {
           return dalUser.CheckAndGetUSer(loginCredentialsDto.Email,loginCredentialsDto.Password);
        }
    }
}
