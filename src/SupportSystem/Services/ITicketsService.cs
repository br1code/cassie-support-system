using SupportSystem.DTOs;

namespace SupportSystem.Services
{
    public interface ITicketsService
    {
        Task AddComment(int id, NewCommentDTO comment);
        Task<int> CreateTicket(NewTicketDTO newTicket);
        Task<TicketDTO> GetTicketByIdAsync(int id);
        Task<IEnumerable<TicketPreviewDTO>> GetTicketsByUser(int userId);
    }
}
