using Microsoft.EntityFrameworkCore;
using ProductUserMicroservice.Data;
using ProductUserMicroservice.Model;

namespace ProductUserMicroservice.Services
{
    public class UserService : IUserService
    {
        private readonly DbContextClass _dbContext;

        public UserService(DbContextClass dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProductOfferDetail> GetProductByIdAsync(int id)
        {
            return await _dbContext.ProductOffers.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductOfferDetail>> GetProductListAsync()
        {
            return await _dbContext.ProductOffers.ToListAsync();
        }

        public async Task<ProductOfferDetail> AddProductAsync(ProductOfferDetail product)
        {
            var result = _dbContext.ProductOffers.Add(product);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }
    }
}
