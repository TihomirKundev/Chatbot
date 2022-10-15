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
        public ActionResult<User> GetUser(CheckUserDTO checkUserDTO)
        {
            User user = userService.GetUser(checkUserDTO);
            if(user.Equals(null))
            {
                return NotFound("Not existing user!");
            }
            else
            {
                return Ok(user);
            }
        }
    }
}
