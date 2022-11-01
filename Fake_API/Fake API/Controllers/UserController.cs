using Fake_API.DTOs;
using Fake_API.Entities;
using Fake_API.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Fake_API.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("user")]
    [EnableCors("Development")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpGet("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<User> GetUser([FromBody]LoginCredentialsDTO loginCredentialsDto)
        {
            return Ok(_userService.GetUser(loginCredentialsDto));
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<User>> GetAll()
        {
            return Ok(_userService.GetAllUsers());
        }

        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<User> GetById([FromBody]GuidDto id)
        {
            return Ok(_userService.GetById(id.id));
        }
        
        [HttpGet("email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<User> GetByEmail([FromBody] EmailDto email) 
        {
            return Ok(_userService.GetByEmail(email.email)); 
        }
    }
}
