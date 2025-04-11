using Microsoft.EntityFrameworkCore;
using CompanyManagementSystem.Web.Models;

namespace CompanyManagementSystem.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TrainingVideo> TrainingVideos { get; set; }
        public DbSet<User> Users { get; set; }
    }
} 