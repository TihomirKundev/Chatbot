using ChatBot.Models.DTOs;
using ChatBot.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Controllers
{
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