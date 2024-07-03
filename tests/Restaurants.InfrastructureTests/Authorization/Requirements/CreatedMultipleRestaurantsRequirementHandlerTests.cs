using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Infrastructure.Authorization.Requirements.Tests
{
    public class CreatedMultipleRestaurantsRequirementHandlerTests
    {
        [Fact()]
        public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldSucceed()
        {
            // arrange
            var id = Guid.NewGuid();
            var currentUser = new CurrentUser(id.ToString(), "test@test.com", [], null, null);

            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(m => m.GetCurrentUser()).Returns(currentUser);

            var restaurants = new List<Restaurant>()
            {
                new()
                {
                    OwnerId = id,
                },
                new()
                {
                    OwnerId = id
                },
                new()
                {
                    OwnerId = Guid.NewGuid(),
                },
            };

            var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
            restaurantRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(restaurants);

            var requirement = new CreatedMultipleRestaurantsRequirement(2);
            var handler = new CreatedMultipleRestaurantsRequirementHandler(restaurantRepositoryMock.Object, userContextMock.Object);
            var context = new AuthorizationHandlerContext([requirement], null, null);

            // act

            await handler.HandleAsync(context);

            // assert

            context.HasSucceeded.Should().BeTrue();
        }

        [Fact()]
        public async Task HandleRequirementAsync_UserHasNotCreatedMultipleRestaurants_ShouldFail()
        {
            // arrange
            var id = Guid.NewGuid();
            var currentUser = new CurrentUser(id.ToString(), "test@test.com", [], null, null);

            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(m => m.GetCurrentUser()).Returns(currentUser);

            var restaurants = new List<Restaurant>()
            {
                new()
                {
                    OwnerId = id,
                },
                new()
                {
                    OwnerId = Guid.NewGuid(),
                },
            };

            var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
            restaurantRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(restaurants);

            var requirement = new CreatedMultipleRestaurantsRequirement(2);
            var handler = new CreatedMultipleRestaurantsRequirementHandler(restaurantRepositoryMock.Object, userContextMock.Object);
            var context = new AuthorizationHandlerContext([requirement], null, null);

            // act

            await handler.HandleAsync(context);

            // assert

            context.HasSucceeded.Should().BeFalse();
            context.HasFailed.Should().BeTrue();
        }
    }
}