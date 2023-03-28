using SupportSystem.Data.Entities;

namespace SupportSystem.DTOs
{
    public class TicketPreviewDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public TicketStatus Status { get; set; }
        public int UserId { get; set; }
    }
}
