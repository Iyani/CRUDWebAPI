using Microsoft.EntityFrameworkCore;

namespace CrudWebAPI.Model
{
    public class BrandContext : DbContext
    {
        public DbSet<Brand> Brands { get; set; }

        public BrandContext(DbContextOptions<BrandContext> options) : base(options)
        {
            
        }
    }
}
