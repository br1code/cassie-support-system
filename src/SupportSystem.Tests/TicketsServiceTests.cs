using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SupportSystem.Data;
using SupportSystem.Data.Entities;
using SupportSystem.DTOs;
using SupportSystem.Services;

namespace SupportSystem.Tests
{
    public class TicketsServiceTests
    {
        private SupportSystemDbContext context;
        private TicketsService service;

        [Fact]
        public async Task GetTicketByIdAsync_ShouldReturnTicket_WhenTicketExists()
        {
            // Arrange
            context = CreateTestContext();

            var ticket = new Ticket
            {
                Id = 1,
                Title = "Test Ticket 1",
                Body = "This is a test ticket.",
                Status = TicketStatus.Open,
                CreatedAt = new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ClosedAt = null,
                UserId = 1,
                Comments = new List<Comment>()
            };

            context.Tickets.Add(ticket);
            await context.SaveChangesAsync();
            service = new TicketsService(context);

            var ticketId = 1;

            // Act
            var result = await service.GetTicketByIdAsync(ticketId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(ticketId);
            result.Title.Should().Be(ticket.Title);
            result.Body.Should().Be(ticket.Body);
            result.Status.Should().Be(ticket.Status);
            result.CreatedAt.Should().Be(ticket.CreatedAt);
            result.ClosedAt.Should().Be(ticket.ClosedAt);
            result.UserId.Should().Be(ticket.UserId);
            result.Comments.Should().HaveCount(ticket.Comments.Count);
        }

        [Fact]
        public async Task GetTicketByIdAsync_ShouldThrowException_WhenTicketDoesNotExist()
        {
            // Arrange
            context = CreateTestContext();
            service = new TicketsService(context);
            var ticketId = 1;

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(async () => await service.GetTicketByIdAsync(ticketId));
        }

        [Fact]
        public async Task GetTicketsByUser_ShouldReturnTickets_WhenTicketsExistForUser()
        {
            // Arrange
            context = CreateTestContext();

            var tickets = new List<Ticket>
            {
                new Ticket
                {
                    Title = "Test ticket 1",
                    Body = "This is a test ticket",
                    UserId = 1,
                    CreatedAt = DateTime.Now,
                    Status = TicketStatus.Open,
                    Comments = new List<Comment>()
                },
                new Ticket
                {
                    Title = "Test ticket 2",
                    Body = "This is a test ticket",
                    UserId = 1,
                    CreatedAt = DateTime.Now,
                    Status = TicketStatus.Open,
                    Comments = new List<Comment>()
                },
                new Ticket
                {
                    Title = "Test ticket 2",
                    Body = "This is a third test ticket",
                    UserId = 2,
                    CreatedAt = DateTime.Now,
                    Status = TicketStatus.Open,
                    Comments = new List<Comment>()
                },
            };

            context.Tickets.AddRange(tickets);
            await context.SaveChangesAsync();
            var service = new TicketsService(context);

            var userId = 1;

            // Act
            var result = await service.GetTicketsByUser(userId);

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(2);

            var firstTicket = tickets.First(t => t.Title == "Test ticket 1");
            var secondTicket = tickets.First(t => t.Title == "Test ticket 2");

            var firstResult = result.First();
            var secondResult = result.Skip(1).First();

            firstResult.Id.Should().Be(firstTicket.Id);
            secondResult.Id.Should().Be(secondTicket.Id);
        }

        [Fact]
        public async Task GetTicketsByUser_ShouldReturnEmpty_WhenNoTicketsForUser()
        {
            // Arrange
            context = CreateTestContext();

            var ticket = new Ticket
            {
                Title = "Test ticket 1",
                Body = "This is a test ticket",
                UserId = 2,
                CreatedAt = DateTime.Now,
                Status = TicketStatus.Open,
                Comments = new List<Comment>()
            };

            context.Tickets.Add(ticket);
            await context.SaveChangesAsync();
            var service = new TicketsService(context);

            var userId = 1;

            // Act
            var result = await service.GetTicketsByUser(userId);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task AddComment_ShouldAddComment_WhenTicketExistsAndIsOpen()
        {
            // Arrange
            context = CreateTestContext();

            var ticket = new Ticket
            {
                Title = "Test ticket 1",
                Body = "This is a test ticket",
                UserId = 1,
                CreatedAt = DateTime.Now,
                Status = TicketStatus.Open,
                Comments = new List<Comment>()
            };

            context.Tickets.Add(ticket);
            await context.SaveChangesAsync();

            service = new TicketsService(context);
            var ticketId = 1;
            var newComment = new NewCommentDTO { Body = "This is a comment" };

            // Act
            await service.AddComment(ticketId, newComment);

            // Assert
            ticket.Comments.Count().Should().Be(1);
            ticket.Comments.First().Id.Should().Be(1);
        }

        [Fact]
        public async Task AddComment_ShouldThrowException_WhenTicketDoesNotExist()
        {
            // Arrange
            context = CreateTestContext();
            service = new TicketsService(context);

            var ticketId = 1;
            var newComment = new NewCommentDTO { Body = "This is a comment" };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await service.AddComment(ticketId, newComment));
        }

        [Fact]
        public async Task AddComment_ShouldThrowException_WHenTicketIsClosed()
        {
            // Arrange
            context = CreateTestContext();

            var ticket = new Ticket
            {
                Title = "Test ticket 1",
                Body = "This is a test ticket",
                UserId = 1,
                CreatedAt = DateTime.Now,
                Status = TicketStatus.Closed,
                Comments = new List<Comment>()
            };

            context.Tickets.Add(ticket);
            await context.SaveChangesAsync();
            service = new TicketsService(context);

            var ticketId = 1;
            var newComment = new NewCommentDTO { Body = "This is a comment" };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await service.AddComment(ticketId, newComment));
        }

        [Fact]
        public async Task CreateTicket_ShouldAddTicket_And_ReturnTicketId()
        {
            // Arrange
            context = CreateTestContext();
            service = new TicketsService(context);

            var newTicket = new NewTicketDTO
            {
                Title = "Test ticket 1",
                Body = "This is a test ticket"
            };

            // Act
            var result = await service.CreateTicket(newTicket);


            // Assert
            context.Tickets.Count().Should().Be(1);
            context.Tickets.First().Title.Should().Be("Test ticket 1");
            context.Tickets.First().Body.Should().Be("This is a test ticket");
            result.Should().Be(1);
        }

        private SupportSystemDbContext CreateTestContext()
        {
            var options = new DbContextOptionsBuilder<SupportSystemDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new SupportSystemDbContext(options);
        }
    }
}