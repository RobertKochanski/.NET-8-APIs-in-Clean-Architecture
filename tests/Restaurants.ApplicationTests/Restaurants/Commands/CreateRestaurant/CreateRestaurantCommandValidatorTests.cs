using FluentValidation.TestHelper;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests
{
    public class CreateRestaurantCommandValidatorTests
    {
        [Fact()]
        public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
        {
            // arrange
            var command = new CreateRestaurantCommand()
            {
                Name = "test",
                Category = "Italian",
                ContactEmail = "test@test.com",
                PostalCode = "12-345"
            };

            var validator = new CreateRestaurantCommandValidator();

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact()]
        public void Validator_ForInvalidCommand_ShouldHaveValidationErrors()
        {
            // arrange
            var command = new CreateRestaurantCommand()
            {
                Name = "te",
                Category = "Ita",
                ContactEmail = "@test.com",
                PostalCode = "12345"
            };

            var validator = new CreateRestaurantCommandValidator();

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldHaveValidationErrorFor(c => c.Name);
            result.ShouldHaveValidationErrorFor(c => c.Category);
            result.ShouldHaveValidationErrorFor(c => c.ContactEmail);
            result.ShouldHaveValidationErrorFor(c => c.PostalCode);
        }

        [Theory()]
        [InlineData("Italian")]
        [InlineData("Mexican")]
        [InlineData("Japanese")]
        [InlineData("American")]
        [InlineData("Indian")]
        public void Validator_ForValidCategory_ShouldNotHaveValidationErrorForCategoryProperty(string category)
        {
            // arrange
            var validator = new CreateRestaurantCommandValidator();
            var command = new CreateRestaurantCommand() { Category = category };

            // act 
            var result = validator.TestValidate(command);

            // assert
            result.ShouldNotHaveValidationErrorFor(c => c.Category);
        }

        [Theory()]
        [InlineData("11202")]
        [InlineData("111-22")]
        [InlineData("10 222")]
        [InlineData("12 32 2")]
        [InlineData("10-0 20")]
        public void Validator_ForInvalidPostalCode_ShouldHaveValidationErrorForPostalCodeProperty(string postalCode)
        {
            // arrange
            var validator = new CreateRestaurantCommandValidator();
            var command = new CreateRestaurantCommand() { PostalCode = postalCode };

            // act 
            var result = validator.TestValidate(command);

            // assert
            result.ShouldHaveValidationErrorFor(c => c.PostalCode);
        }
    }
}