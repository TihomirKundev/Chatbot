using Fake_API.DTOs;
using Fake_API.Service;
using FakeAPI.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Fake_API.Controllers
{
    [ApiController]
    [Route("user")]
    [EnableCors("Development")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<User> GetUser(LoginCredentialsDTO loginCredentialsDto)
        {
            return Ok(_userService.GetUser(loginCredentialsDto));
        }
    }
}
