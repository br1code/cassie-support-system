using Microsoft.EntityFrameworkCore;
using SupportSystem.Data.Entities;

namespace SupportSystem.Data
{
    public class SupportSystemDbContext : DbContext
    {
        public SupportSystemDbContext(DbContextOptions<SupportSystemDbContext> options) : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
