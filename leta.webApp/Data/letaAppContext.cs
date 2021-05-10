using Microsoft.EntityFrameworkCore;

namespace leta.webApp.Data
{
    public class LetaAppContext : DbContext
    {
        public LetaAppContext(DbContextOptions<LetaAppContext> options) : base(options)
        {
        }
        public DbSet<RouteTime> RouteTime { get; set; }
    }
}
