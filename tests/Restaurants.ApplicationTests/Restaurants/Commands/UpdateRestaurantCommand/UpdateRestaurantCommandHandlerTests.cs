using AutoMapper;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Domain.Contants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurantCommand.Tests
{
    public class UpdateRestaurantCommandHandlerTests
    {
        private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
        private readonly Mock<IRestaurantsRepository> _restaurantRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationServiceMock;

        private readonly UpdateRestaurantCommandHandler _handler;

        public UpdateRestaurantCommandHandlerTests()
        {
            _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
            _mapperMock = new Mock<IMapper>();
            _restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
            _restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();

            _handler = new UpdateRestaurantCommandHandler(_loggerMock.Object, _restaurantRepositoryMock.Object,
                _mapperMock.Object, _restaurantAuthorizationServiceMock.Object);
        }

        [Fact()]
        public async Task Handle_WithValidRequest_ShouldUpdateRestaurants()
        {
            // arrange

            var restaurantId = Guid.NewGuid();
            var restaurant = new Restaurant()
            {
                Id = restaurantId,
                Name = "Name",
                Description = "Description",
            };
            var command = new UpdateRestaurantCommand()
            {
                Id = restaurantId,
                Name = "New name",
                Description = "New description",
                HasDelivery = true,
            };

            _restaurantRepositoryMock.Setup(r => r.GetByIdAsync(restaurantId)).ReturnsAsync(restaurant);

            _restaurantAuthorizationServiceMock.Setup(r => r.Authorize(restaurant, ResourceOperation.Update)).Returns(true);

            // act

            await _handler.Handle(command, CancellationToken.None);

            // assert

            _restaurantRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
            _mapperMock.Verify(m => m.Map(command, restaurant), Times.Once);
        }

        [Fact()]
        public async Task Handle_WithNonExistingRestaurant_ShouldThrowNotFoundException()
        {
            // arrange
            var request = new UpdateRestaurantCommand()
            {
                Id = Guid.NewGuid(),
            };

            _restaurantRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Restaurant?)null);

            // act
            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            // assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Restaurant with id: {request.Id} doesn't exist");
        }

        [Fact()]
        public async Task Handle_WithUnauthorizedUser_ShouldThrowForbidException()
        {
            // arrange
            var restaurantId = Guid.NewGuid();
            var restaurant = new Restaurant()
            {
                Id = restaurantId,
            };
            var request = new UpdateRestaurantCommand()
            {
                Id = restaurantId,
            };

            _restaurantRepositoryMock.Setup(r => r.GetByIdAsync(restaurantId)).ReturnsAsync(restaurant);

            _restaurantAuthorizationServiceMock.Setup(r => r.Authorize(restaurant, ResourceOperation.Update)).Returns(false);

            // act
            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            // assert
            await act.Should().ThrowAsync<ForbidException>();
        }
    }
}