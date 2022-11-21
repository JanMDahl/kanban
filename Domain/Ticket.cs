using KanbanAPI.Controllers.Requests;

namespace KanbanAPI.Domain
{
    public class Ticket
    {
        public int Id { get; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public Status Status { get; private set; }
        public Priority Priority { get; private set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ClosedAt { get; private set; }

        public Ticket(int id, string title, string description, Status status, Priority priority, DateTime createdAt, DateTime updatedAt, DateTime? closedAt)
        {
            Id = id;
            Title = title;
            Description = description;
            Status = status;
            Priority = priority;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            ClosedAt = closedAt;
        }

        public Ticket(CreateTicketRequest request)
        {
            Title = request.Title;
            Description = request.Description;
            Priority = (Priority)request.Priority;
            Status = Status.Open;
            CreatedAt = DateTime.Now;
            UpdatedAt = CreatedAt;
            ClosedAt = null;
        }

        public void Update(string? title, string? description, Status? status, Priority? priority)
        {
            if (status == Status.Closed && Status != Status.Closed)
            {
                ClosedAt = DateTime.Now;
            }
            else if (Status == Status.Closed && status != Status.Closed)
            {
                ClosedAt = null;
            }

            Title = title ?? Title;
            Description = description ?? Description;
            Status = status ?? Status;
            Priority = priority ?? Priority;
            UpdatedAt = DateTime.Now;
        }

        public void Close()
        {
            Status = Status.Closed;
            ClosedAt = DateTime.Now;
        }
    }
    
    public enum Priority
    {
        Low,    // 0
        Medium, // 1
        High    // 2
    }

    public enum Status
    {
        Open,       // 0
        InProgress, // 1
        Closed      // 2  
    }
}
