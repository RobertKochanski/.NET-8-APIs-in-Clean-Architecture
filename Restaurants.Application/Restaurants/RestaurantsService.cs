﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants
{
    internal class RestaurantsService(IRestaurantsRepository restaurantsRepository,
        ILogger<RestaurantsService> logger,
        IMapper mapper) 
        : IRestaurantsService
    {
        public async Task<Guid> Create(CreateRestaurantDto dto)
        {
            logger.LogInformation("Creating a new restaurant");
            var restaurant = mapper.Map<Restaurant>(dto);

            var id = await restaurantsRepository.Create(restaurant);
            return id;
        }

        public async Task<IEnumerable<RestaurantDto>> GetAllRestaurants()
        {
            logger.LogInformation("Getting all restaurants");
            var restaurants = await restaurantsRepository.GetAllAsync();

            var restaurantsDtos = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

            return restaurantsDtos!;
        }
        
        public async Task<RestaurantDto?> GetById(Guid id)
        {
            logger.LogInformation("Getting restaurant with " + id + " id");

            var restaurant = await restaurantsRepository.GetByIdAsync(id);

            var restaurantDto = mapper.Map<RestaurantDto?>(restaurant);

            return restaurantDto;
        }
    }
}
