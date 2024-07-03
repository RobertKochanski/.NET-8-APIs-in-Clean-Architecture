using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurantCommand
{
    public class UpdateRestaurantCommandValidator : AbstractValidator<UpdateRestaurantCommand>
    {
        public UpdateRestaurantCommandValidator()
        {
            RuleFor(c => c.Name)
                .Length(3, 100);
        }
    }
}
