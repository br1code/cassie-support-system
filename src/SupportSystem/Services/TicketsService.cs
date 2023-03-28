using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SupportSystem.Data;
using SupportSystem.Data.Entities;
using SupportSystem.DTOs;
using System;

namespace SupportSystem.Services
{
    public class TicketsService : ITicketsService
    {
        private readonly SupportSystemDbContext context;

        public TicketsService(SupportSystemDbContext _context)
        {
            context = _context;
        }

        public async Task AddComment(int id, NewCommentDTO comment)
        {
            // TODO: validate ticket owner, receive user id and check
            var userId = 1;
            var ticket = await context.Tickets
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(t => t.Id == id && t.Status == TicketStatus.Open);

            if (ticket == null)
            {
                throw new Exception($"Ticket with id {id} not found.");
            }


            var newComment = new Comment
            {
                Body = comment.Body,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            ticket.Comments.Add(newComment);
            await context.SaveChangesAsync();
        }

        public async Task<int> CreateTicket(NewTicketDTO newTicket)
        {
            // TODO: get current user
            var userId = 1;
            var ticket = new Ticket
            {
                Title = newTicket.Title,
                Body = newTicket.Body,
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };

            context.Tickets.Add(ticket);
            await context.SaveChangesAsync();

            return ticket.Id;
        }

        public async Task<TicketDTO> GetTicketByIdAsync(int id)
        {
            var ticket = await context.Tickets
                .AsNoTracking()
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null)
            {
                throw new Exception($"Ticket with id {id} not found.");
            }

            return new TicketDTO
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Body = ticket.Body,
                CreatedAt = ticket.CreatedAt,
                ClosedAt = ticket.ClosedAt,
                Status = ticket.Status,
                UserId = ticket.UserId,
                Comments = ticket.Comments.Select(comment => new CommentDTO
                {
                    Id = comment.Id,
                    UserId = comment.UserId,
                    Body = comment.Body,
                    CreatedAt = comment.CreatedAt
                }).ToList()
            };
        }

        public async Task<IEnumerable<TicketPreviewDTO>> GetTicketsByUser(int userId)
        {
            var tickets = await context.Tickets
                .AsNoTracking()
                .Where(t => t.UserId == userId)
                .ToListAsync();

            return tickets.Select(t => new TicketPreviewDTO
            {
                Id = t.Id,
                Title = t.Title,
                CreatedAt = t.CreatedAt,
                Status = t.Status,
                UserId = t.UserId
            });
        }
    }
}
