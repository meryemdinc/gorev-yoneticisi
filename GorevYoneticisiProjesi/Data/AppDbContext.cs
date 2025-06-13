using GorevYoneticisiProjesi.Models;
using Microsoft.EntityFrameworkCore;

namespace GorevYoneticisiProjesi.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<TaskReport> TaskReports { get; set; }
    }
}
