using Microsoft.EntityFrameworkCore;
using ProductOwnerMicroservice.Models;

namespace ProductOwnerMicroservice.Data
{
    public class DbContextClass : DbContext
    {
        protected readonly IConfiguration Configuration;
        public DbContextClass(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring (DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));

        }

        public DbSet<ProductDetails> Products { get; set; }
    }
}
