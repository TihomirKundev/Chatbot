using ChatBot.Auth.Jwt;
using ChatBot.Exceptions;
using ChatBot.Extensions;
using ChatBot.Models.Request;
using ChatBot.Models.Response;
using ChatBot.Repositories.Interfaces;
using ChatBot.Services.Interfaces;

namespace ChatBot.Services;

[TransientService]
public class AuthService : IAuthService
{
    private readonly IJwtUtils _utils;
    private readonly IUserService _userService;

    public AuthService(IJwtUtils utils, IUserService userService)
    {
        _utils = utils;
        _userService = userService;
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest request)
    {
        var account = _userService.GetByEmail(request.Email);

        if (account == null || !BCrypt.Net.BCrypt.Verify(request.Password, account.Password))
            throw new InvalidCredentialsException("Invalid credentials");

        var token = _utils.GenerateToken(account);
        return new AuthenticateResponse(account, token);
    }
}