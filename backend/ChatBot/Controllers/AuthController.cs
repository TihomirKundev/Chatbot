using ChatBot.Auth.Helpers;
using ChatBot.Models.Request;
using ChatBot.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Controllers;

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