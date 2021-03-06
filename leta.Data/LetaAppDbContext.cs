using leta.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace leta.Data
{
    public class LetaAppDbContext : DbContext
    {
        public LetaAppDbContext(DbContextOptions<LetaAppDbContext> options) : base(options)
        {
        }
        public DbSet<RouteTime> RouteTime { get; set; }
        public DbSet<InfoModel> InfoModel { get; set; }
    }
}
