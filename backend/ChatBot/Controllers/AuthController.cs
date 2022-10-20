using ChatBot.Auth.Helpers;
using ChatBot.Models;
using ChatBot.Models.DTOs;
using ChatBot.Services;
using ChatBot.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot;

[Authorize]
[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly AppSettings _settings;
    
    public AuthController(AppSettings settings, IAuthService authService)
    {
        _settings = settings;
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public ActionResult<AuthenticateRequest> Login([FromBody] AuthenticateRequest request)
    {
        var response = _authService.Authenticate(request);
        return Ok(response);
    }
}