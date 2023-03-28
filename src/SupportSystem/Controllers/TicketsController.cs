using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SupportSystem.DTOs;
using SupportSystem.Services;

namespace SupportSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketsService ticketsService;
        private readonly IValidator<NewTicketDTO> ticketValidator;
        private readonly IValidator<NewCommentDTO> commentValidator;
        private readonly ILogger<TicketsController> logger;

        public TicketsController(ITicketsService _ticketsService,
            IValidator<NewTicketDTO> _ticketValidator,
            IValidator<NewCommentDTO> _commentValidator,
            ILogger<TicketsController> _logger)
        {
            ticketsService = _ticketsService;
            ticketValidator = _ticketValidator;
            commentValidator = _commentValidator;
            logger = _logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketPreviewDTO>>> GetTickets()
        {
            // TODO: get information about the user so we can decide what to return
            // in order to fetch the tickets from a specific user, we need the user id
            // if we use this same endpoint for employees, we would need something else
            var userId = 1;
            var tickets = await ticketsService.GetTicketsByUser(userId);

            // TODO: should we return an empty list or a NotFound error status code?

            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDTO>> GetTicket(int id)
        {
            // TODO: validation
            // when user: get user context and validate that the user owns the requested ticket
            // when employee: not required
            var ticket = await ticketsService.GetTicketByIdAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return Ok(ticket);
        }

        [HttpPost()]
        public async Task<ActionResult<int>> CreateTicket([FromBody] NewTicketDTO newTicket)
        {
            var result = ticketValidator.Validate(newTicket);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            var ticketId = await ticketsService.CreateTicket(newTicket);
            return Ok(ticketId);
        }

        [HttpPost("{id}/comments")]
        public async Task<IActionResult> AddComment(int id, [FromBody] NewCommentDTO newComment)
        {
            var result = commentValidator.Validate(newComment);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            await ticketsService.AddComment(id, newComment);
            return Ok();
        }
    }
}
