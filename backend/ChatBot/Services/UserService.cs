using System;
using System.Collections.Generic;
using System.Linq;
using ChatBot.Auth;
using ChatBot.Auth.Exception.CustomExceptions;
using ChatBot.Auth.Repository;
using ChatBot.Models;
using ChatBot.Models.DTOs;
using ChatBot.Models.Response;


namespace ChatBot.Services;

public class UserService
{
    private readonly JwtUtils _jwtUtils;
    private readonly IUserRepository _userRepository;

    public UserService(JwtUtils jwtUtils, IUserRepository userRepository)
    {
        _jwtUtils = jwtUtils;
        _userRepository = userRepository;
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest request)
    {
        var user = _userRepository.GetByEmail(request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            throw new InvalidCredentialsException("Invalid credentials");
        
        var token = _jwtUtils.GenerateToken(user);
        return new AuthenticateResponse(user, token);     
    }


    public User GetUserById(Guid userId)
    {
        var user = _userRepository.GetById(userId);
        if(user is null)
            throw new UserNotFoundException("User not found");
        return user; 
    }
   
    public void Register(RegisterRequest request)
    {
        if (_userRepository.GetAll().Any(x => x.Email == request.Email))
            throw new DuplicateEmailException("Email already exists");
        
        string password = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User(Guid.NewGuid(), 
            request.FirstName, 
            request.LastName,
            password,
            request.Email,
            request.Phone,
            (Role) Enum.Parse(typeof(Role), request.Role));
        
        _userRepository.Save(user);
    }

    public List<User> GetAllUsers()
    {
        return _userRepository.GetAll();
    }
}





