using ChatBot.Models;
using ChatBot.Models.Request;
using ChatBot.Models.Response;

namespace ChatBot.Services.Interfaces;

public interface IAuthService
{
    AuthenticateResponse Authenticate(AuthenticateRequest request);
    User? CheckToken(string token);
}