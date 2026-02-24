using Microsoft.EntityFrameworkCore;
using ClickTrackerAPI.Models;

namespace ClickTrackerAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ClickEvent> ClickEvents { get; set; }
    }
}
