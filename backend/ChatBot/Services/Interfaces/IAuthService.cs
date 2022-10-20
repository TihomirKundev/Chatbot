using ChatBot.Models.DTOs;
using ChatBot.Models.Response;

namespace ChatBot.Services.Interfaces;

public interface IAuthService
{
    AuthenticateResponse Authenticate(AuthenticateRequest request);
}