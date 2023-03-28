namespace SupportSystem.Data.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
    }
}
