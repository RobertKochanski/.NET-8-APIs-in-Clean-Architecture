using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests
{
    public class CreateRestaurantCommandHandlerTests
    {
        [Fact()]
        public async void Handle_ForValidCommand_ReturnsCreatedRestaurantId()
        {
            // arrange
            var id = Guid.NewGuid();

            var loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();
            var mapperMock = new Mock<IMapper>();

            var command = new CreateRestaurantCommand();
            var restaurant = new Restaurant();
            
            mapperMock.Setup(m => m.Map<Restaurant>(command)).Returns(restaurant);
            
            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser(Guid.NewGuid().ToString(), "test@test.com", [], null, null);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
            restaurantRepositoryMock.Setup(repo => repo.Create(It.IsAny<Restaurant>())).ReturnsAsync(id);

            var commandHandler = new CreateRestaurantCommandHandler(mapperMock.Object, loggerMock.Object,
                restaurantRepositoryMock.Object, userContextMock.Object);

            // act 
            var result = await commandHandler.Handle(command, CancellationToken.None);

            // assert
            result.Should().Be(id);
            restaurant.OwnerId.Should().Be(currentUser.Id);
            restaurantRepositoryMock.Verify(r => r.Create(restaurant), Times.Once());
        }
    }
}