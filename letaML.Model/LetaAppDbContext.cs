using Microsoft.EntityFrameworkCore;

namespace leta.Model
{
    public class LetaAppDbContext : DbContext
    {
        public LetaAppDbContext(DbContextOptions<LetaAppDbContext> options) : base(options)
        {
        }
        public DbSet<RouteTime> RouteTime { get; set; }
    }
}
