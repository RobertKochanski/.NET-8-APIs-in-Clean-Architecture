using Microsoft.AspNetCore.Identity;

namespace Restaurants.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public DateOnly? DateOfBirth { get; set; }
        public string? Nationality { get; set; }
    }
}
