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
    private readonly IAccountRepository _accountRepo;

    public AuthService(IJwtUtils utils, IAccountRepository accountRepo)
    {
        _utils = utils;
        _accountRepo = accountRepo;
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest request)
    {
        var account = _accountRepo.GetByEmail(request.Email); //change to http call to fake api

        if (account == null || !BCrypt.Net.BCrypt.Verify(request.Password, account.Password))
            throw new InvalidCredentialsException("Invalid credentials");

        var token = _utils.GenerateToken(account);
        return new AuthenticateResponse(account, token);
    }
}