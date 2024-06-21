﻿using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Contants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Infrastructure.Authorization.Services
{
    public class RestaurantAuthorizationService(ILogger<RestaurantAuthorizationService> logger,
        IUserContext userContext) : IRestaurantAuthorizationService
    {
        public bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation)
        {
            var user = userContext.GetCurrentUser();

            logger.LogInformation("Authorizing user {UserEmail}, to {Operation} for restaurant {RestaurantName}",
                user.Email, resourceOperation, restaurant.Name);

            if (resourceOperation == ResourceOperation.Read || resourceOperation == ResourceOperation.Create)
            {
                logger.LogInformation("Create/read operation - succesful authorization");
                return true;
            }

            if (resourceOperation == ResourceOperation.Delete || user.IsInRole(UserRoles.Admin))
            {
                logger.LogInformation("Admin user, delete operation - succesful authorization");
                return true;
            }

            if ((resourceOperation == ResourceOperation.Delete || resourceOperation == ResourceOperation.Update)
                && Guid.Parse(user.Id) == restaurant.OwnerId)
            {
                logger.LogInformation("Restaurant owner - succesful authorization");
                return true;
            }

            return false;
        }
    }
}
