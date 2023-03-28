namespace SupportSystem.Data.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public TicketStatus Status { get; set; }
        public int UserId { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
 