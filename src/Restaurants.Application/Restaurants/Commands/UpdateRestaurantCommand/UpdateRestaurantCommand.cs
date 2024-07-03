using MediatR;
using System.Text.Json.Serialization;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurantCommand
{
    public class UpdateRestaurantCommand : IRequest
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public bool HasDelivery { get; set; }
    }
}
