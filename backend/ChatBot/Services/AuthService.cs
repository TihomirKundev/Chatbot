using ChatBot.Auth.Jwt;
using ChatBot.Exceptions;
using ChatBot.Extensions;
using ChatBot.Http;
using ChatBot.Models.Request;
using ChatBot.Models.Response;
using ChatBot.Services.Interfaces;

namespace ChatBot.Services;

[TransientService]
public class AuthService : IAuthService
{
    private readonly IJwtUtils _utils;
    private readonly IUserService _userService;
    private readonly IFakeApiHttpClient _httpClient;

    public AuthService(IJwtUtils utils, IUserService userService, IFakeApiHttpClient httpClient)
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
}