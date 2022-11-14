using System;
using ChatBot.Auth.Attributes;
using ChatBot.Models;
using ChatBot.Services.Interfaces;
using Fake_API.Entities.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Controllers;

/// <summary>
/// Added for testing purposes
///

[ApiController]
[Route("user")] 
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("id")]
    [AllowAnonymous]
    public ActionResult<User> GetById([FromBody] GuidDto id)
    {
        return Ok(_userService.GetById(id.id));
    }
    
    [HttpGet("email")]
    [AllowAnonymous]
    public ActionResult<User> GetByEmail([FromBody] EmailDto email)
    {
        return Ok(_userService.GetByEmail(email.email));
    }
    
    
    [HttpGet("all")]
    [Auth.Attributes.Authorize]
    public ActionResult<User> GetAll()
    {
        return Ok(_userService.GetAllUsers());
    }
}