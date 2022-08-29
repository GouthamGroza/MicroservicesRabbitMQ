using Microsoft.EntityFrameworkCore;
using ProductUserMicroservice.Model;

namespace ProductUserMicroservice.Data
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
        public DbSet<ProductOfferDetail> ProductOffers { get; set; }
    }
}
