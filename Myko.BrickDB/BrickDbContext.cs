using Microsoft.EntityFrameworkCore;

namespace Myko.BrickDB
{
    public class BrickDbContext : DbContext
    {
        public DbSet<Color> Colors { get; set; } = null!;
        public DbSet<Design> Designs { get; set; } = null!;
        public DbSet<Element> Elements { get; set; } = null!;

        public BrickDbContext(DbContextOptions options)
            : base(options)
        {

        }
    }
}
