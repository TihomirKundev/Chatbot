using ChatBot.Auth.Jwt;
using ChatBot.Exceptions;
using ChatBot.Extensions;
using ChatBot.Http;
using ChatBot.Models;
using ChatBot.Models.Request;
using ChatBot.Models.Response;
using ChatBot.Services.Interfaces;
using System;

namespace ChatBot.Services;

[TransientService]
public class AuthService : IAuthService
{
    private readonly IJwtUtils _utils;
    private readonly IUserService _userService;
    private readonly IUserHttpClient _httpClient;

    public AuthService(IJwtUtils utils, IUserService userService, IUserHttpClient httpClient)
    {
        _utils = utils;
        _userService = userService;
        _httpClient = httpClient;
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest request)
    {
        var userRequest = _httpClient.GetUserAsync(request);
        var user = userRequest.Result;

        if (user is null)
            throw new UserNotFoundException("Invalid email or password");

        var token = _utils.GenerateToken(user);
        return new AuthenticateResponse(user, token);
    }

    public User? CheckToken(string token)
    {
        return _utils.ValidateToken(token) is Guid userID && _userService.GetById(userID) is var user ? user : null;
    }
}