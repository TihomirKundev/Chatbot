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
        private UserService userService;

        public UserController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<User> GetUser(LoginCredentialsDTO loginCredentialsDto)
        {
            User? user = userService.GetUser(loginCredentialsDto);
            if(user is null)
            {
                return BadRequest("wrong credentials");
            }
            return Ok(user);
        }
    }
}
