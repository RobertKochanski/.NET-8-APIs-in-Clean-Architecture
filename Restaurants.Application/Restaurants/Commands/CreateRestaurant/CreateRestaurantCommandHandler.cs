using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant
{
    public class CreateRestaurantCommandHandler(
        IMapper mapper, 
        ILogger<CreateRestaurantCommandHandler> logger,
        IRestaurantsRepository restaurantsRepository) : IRequestHandler<CreateRestaurantCommand, Guid>
    {
        public async Task<Guid> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating a new restaurant");
            var restaurant = mapper.Map<Restaurant>(request);

            var id = await restaurantsRepository.Create(restaurant);
            return id;
        }
    }
}
