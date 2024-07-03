using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Contants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;
using System.Linq.Expressions;

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

        public async Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchPhase, int pageNumber, int pageSize,
            string? sortBy, SortDirection sortDirection)
        {
            var searchPhaseLower = searchPhase?.ToLower();

            var baseQuery = dbContext.Restaurants
                .Include(x => x.Dishes)
                .Where(x => searchPhaseLower == null || x.Name.ToLower().Contains(searchPhaseLower) || x.Description.ToLower().Contains(searchPhaseLower));

            var totalCount = await baseQuery.CountAsync();

            if (sortBy != null)
            {
                var columnSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
                {
                    { nameof(Restaurant.Name), x => x.Name },
                    { nameof(Restaurant.Description), x => x.Description },
                    { nameof(Restaurant.Category), x => x.Category },
                };

                var selectedColumn = columnSelector[sortBy];

                baseQuery = sortDirection == SortDirection.Ascending
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }

            var restaurants = await baseQuery
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (restaurants, totalCount);
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
