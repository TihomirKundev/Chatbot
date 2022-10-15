using ChatBot.Auth.Helpers;
using ChatBot.Models;
using ChatBot.Models.DTOs;
using ChatBot.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot;

[Authorize]
[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly AppSettings _settings;
    
    public AuthController(UserService userService, AppSettings settings)
    {
        _userService = userService;
        _settings = settings;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public ActionResult<AuthenticateRequest> Login([FromBody] AuthenticateRequest request)
    {
        var response = _userService.Authenticate(request);
        return Ok(response);
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public ActionResult<AuthenticateRequest> Register([FromBody] RegisterRequest request)
    {
        _userService.Register(request);
        return Ok(new {message = "User registered successfully"});
    }
    
    [AllowAnonymous]
    [HttpGet("all")]
    public ActionResult<User> GetAll()
    {
        var users = _userService.GetAllUsers();
        return Ok(users);
    }
}