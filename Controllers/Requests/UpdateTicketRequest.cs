

namespace KanbanAPI.Controllers.Requests
{
    public class UpdateTicketRequest
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public int? Priority { get; set; }
    }
}
