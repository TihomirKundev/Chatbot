﻿using ChatBot.Models.DTOs;
using ChatBot.Services;
using ChatBot.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot
{
    [ApiController]
    [Route("ticket")] //might need [] around ticket
    [EnableCors("Development")]
    public class TicketController : ControllerBase
    {
        private ITicketService _ticketManager;

        public TicketController(ITicketService ticketManager)
        {
            _ticketManager = ticketManager;
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<TicketDTO> Create(TicketCreateDTO ticketReq)
        {
            TicketDTO ticket = _ticketManager.CreateTicket(ticketReq);
            return CreatedAtAction(nameof(Create), ticket);
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<TicketDTO[]> GetAll()
        {
            TicketDTO[] tickets = _ticketManager.GetAllTickets();
            return Ok(tickets);
        }

        [HttpOptions]
        public IActionResult PreflightRoute()
        {
            return NoContent();
        }
    }
}
