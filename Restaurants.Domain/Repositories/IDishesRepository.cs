using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories
{
    public interface IDishesRepository
    {
        Task<Guid> CreateDish(Dish dish);
        Task DeleteAllDishes(IEnumerable<Dish> dishes);
    }
}
