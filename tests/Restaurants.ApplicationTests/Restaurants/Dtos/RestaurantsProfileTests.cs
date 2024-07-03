using AutoMapper;
using FluentAssertions;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurantCommand;
using Restaurants.Domain.Entities;
using Xunit;

namespace Restaurants.Application.Restaurants.Dtos.Tests
{
    public class RestaurantsProfileTests
    {
        private IMapper _mapper;
        private Guid id = Guid.NewGuid();

        public RestaurantsProfileTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RestaurantsProfile>();
            });
            _mapper = configuration.CreateMapper();
        }

        [Fact()]
        public void CreateMap_ForRestaurantToRestaurantDto_MapsCorrectly()
        {
            // arrange
            var restaurant = new Restaurant()
            {
                Id = id,
                Name = "Test restaurant",
                Description = "Test description",
                Category = "Test category",
                HasDelivery = true,
                ContactEmail = "test@example.com",
                ContactNumber = "123456789",
                Address = new Address()
                {
                    City = "Test city",
                    Street = "Test street",
                    PostalCode = "11-222"
                }
            };

            // act
            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

            // assert
            restaurantDto.Should().NotBeNull();
            restaurantDto.Id.Should().Be(restaurant.Id);
            restaurantDto.Name.Should().Be(restaurant.Name);
            restaurantDto.Description.Should().Be(restaurant.Description);
            restaurantDto.Category.Should().Be(restaurant.Category);
            restaurantDto.HasDelivery.Should().Be(restaurant.HasDelivery);
            restaurantDto.City.Should().Be(restaurant.Address.City);
            restaurantDto.Street.Should().Be(restaurant.Address.Street);
            restaurantDto.PostalCode.Should().Be(restaurant.Address.PostalCode);
        }

        [Fact()]
        public void CreateMap_ForCreateRestaurantToRestaurant_MapsCorrectly()
        {
            var command = new CreateRestaurantCommand()
            {
                Name = "Test restaurant",
                Description = "Test description",
                Category = "Test category",
                HasDelivery = true,
                ContactEmail = "test@example.com",
                ContactNumber = "123456789",
                City = "Test city",
                Street = "Test street",
                PostalCode = "11-222"
            };

            // act
            var restaurant = _mapper.Map<Restaurant>(command);

            // assert
            restaurant.Should().NotBeNull();
            restaurant.Name.Should().Be(command.Name);
            restaurant.Description.Should().Be(command.Description);
            restaurant.Category.Should().Be(command.Category);
            restaurant.HasDelivery.Should().Be(command.HasDelivery);
            restaurant.ContactNumber.Should().Be(command.ContactNumber);
            restaurant.ContactEmail.Should().Be(command.ContactEmail);
            restaurant.Address.Should().NotBeNull();
            restaurant.Address.City.Should().Be(command.City);
            restaurant.Address.Street.Should().Be(command.Street);
            restaurant.Address.PostalCode.Should().Be(command.PostalCode);
        }

        [Fact()]
        public void CreateMap_ForUpdateRestaurantToRestaurant_MapsCorrectly()
        {
            var command = new UpdateRestaurantCommand()
            {
                Id = id,
                Name = "Updated restaurant",
                Description = "Updated description",
                HasDelivery = false,
            };

            // act
            var restaurant = _mapper.Map<Restaurant>(command);

            // assert
            restaurant.Should().NotBeNull();
            restaurant.Id.Should().Be(id);
            restaurant.Name.Should().Be(command.Name);
            restaurant.Description.Should().Be(command.Description);
            restaurant.HasDelivery.Should().Be(command.HasDelivery);
        }
    }
}