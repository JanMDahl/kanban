using KanbanAPI.Controllers.Requests;
using KanbanAPI.Domain;
using KanbanAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace KanbanAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketController(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Ticket>>> GetTickets()
        {
            return await _ticketRepository.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var ticket = await _ticketRepository.GetById(id);

            if (ticket is null)
            {
                return NotFound("Ticket not found.");
            }

            return ticket;
        }

        [HttpPost]
        public async Task CreateTicket(CreateTicketRequest request)
        {
            await _ticketRepository.Upsert(new Ticket(request));
        }

        [HttpPatch]
        public async Task<ActionResult<Ticket>> UpdateTicket(UpdateTicketRequest request)
        {
            var ticket = await _ticketRepository.GetById(request.Id);

            if (ticket is null)
            {
                return NotFound("Ticket not found.");
            }

            ticket.Update(request.Title, request.Description, (Status?)request.Status, (Priority?)request.Priority);

            await _ticketRepository.Upsert(ticket);

            return ticket;
        }

        [HttpDelete]
        public async Task DeleteTicket(int id)
        {
            await _ticketRepository.Delete(id);
        }
    }
}