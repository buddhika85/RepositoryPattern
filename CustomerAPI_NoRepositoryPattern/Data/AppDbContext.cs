using Microsoft.EntityFrameworkCore;
using Models;

namespace CustomerAPI_NoRepositoryPattern.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // tables
        public DbSet<Customer> Customers => Set<Customer>();
    }
}
