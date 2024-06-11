using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories
{
    internal class RestaurantsRepository(RestaurantsDbContext dbContext) : IRestaurantsRepository
    {
        public async Task<Guid> Create(Restaurant entity)
        {
            await dbContext.Restaurants.AddAsync(entity);
            await dbContext.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            var restaurants = await dbContext.Restaurants.Include(x => x.Dishes).ToListAsync();
            return restaurants;
        }
        
        public async Task<Restaurant?> GetByIdAsync(Guid id)
        {
            var restaurant = await dbContext.Restaurants.Include(x => x.Dishes).FirstOrDefaultAsync(x => x.Id == id);

            return restaurant;
        }

        public async Task Delete(Restaurant restaurant)
        {
            dbContext.Remove(restaurant);
            await dbContext.SaveChangesAsync();
        }

        public Task SaveChanges() 
            => dbContext.SaveChangesAsync();
    }
}
