using ChatBot.Auth.Attributes;
using ChatBot.Models.DTOs;
using ChatBot.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Controllers
{
    /// <summary>
    /// When a func is annotated with [Token] attribute, it requires a token to be passed in the header 
    ///
    [ApiController]
    [Route("ticket")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost("")]
        [AllowAnonymous]
        public ActionResult<TicketDTO> Create(TicketCreateDTO ticketReq)
        {
            return CreatedAtAction(nameof(Create), _ticketService.CreateTicket(ticketReq));
        }

        [HttpGet("")]
        [AllowAnonymous]
        public ActionResult<TicketDTO[]> GetAll()
        {
            return Ok(_ticketService.GetAllTickets());
        }
    }
}