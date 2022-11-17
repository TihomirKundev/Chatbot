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
/// When a func is annotated with [Token] attribute, it requires a token to be passed in the header 
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
    [Token]
    public ActionResult<User> GetById([FromBody] GuidDto id)
    {
        return Ok(_userService.GetById(id.id));
    }
    
    [HttpGet("email")]
    [Token]
    public ActionResult<User> GetByEmail([FromBody] EmailDto email)
    {
        return Ok(_userService.GetByEmail(email.email));
    }
    
    
    [HttpGet("all")]
    [Token]
    public ActionResult<User> GetAll()
    {
        return Ok(_userService.GetAllUsers());
    }
}