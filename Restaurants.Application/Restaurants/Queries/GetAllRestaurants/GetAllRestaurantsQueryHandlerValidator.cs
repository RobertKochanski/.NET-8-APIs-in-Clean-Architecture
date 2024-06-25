using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQueryHandlerValidator : AbstractValidator<GetAllRestaurantsQuery>
    {
        private int[] allowPageSizes = [5, 10, 15, 30];
        private string[] allowSortByColumnNames = [nameof(RestaurantDto.Name),
            nameof(RestaurantDto.Description), nameof(RestaurantDto.Category)];

        public GetAllRestaurantsQueryHandlerValidator()
        {
            RuleFor(r => r.PageNumber)
                .GreaterThanOrEqualTo(1);

            RuleFor(r => r.PageSize)
                .Must(value => allowPageSizes.Contains(value))
                .WithMessage($"Page size must be in [{string.Join(",", allowPageSizes)}]");

            RuleFor(r => r.SortBy)
                .Must(value => allowSortByColumnNames.Contains(value))
                .When(q => q.SortBy != null)
                .WithMessage($"Sort by is optional, or must be in [{string.Join(",", allowSortByColumnNames)}]");
        }
    }
}
