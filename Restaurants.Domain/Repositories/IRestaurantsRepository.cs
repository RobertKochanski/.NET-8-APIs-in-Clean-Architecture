using Restaurants.Domain.Contants;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories
{
    public interface IRestaurantsRepository
    {
        Task<IEnumerable<Restaurant>> GetAllAsync();
        Task<Restaurant?> GetByIdAsync(Guid id);
        Task<Guid> Create(Restaurant entity);

        Task Delete(Restaurant entity);
        Task SaveChanges();

        Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchPhase, int pageNumber, int pageSize, string? SortBy,
            SortDirection sortDirection);
    }
}
