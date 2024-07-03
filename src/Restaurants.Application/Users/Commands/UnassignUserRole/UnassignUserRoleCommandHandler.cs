using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.UnassignUserRole
{
    public class UnassignUserRoleCommandHandler(ILogger<UnassignUserRoleCommandHandler> logger,
        UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager) : IRequestHandler<UnassignUserRoleCommand>
    {
        public async Task Handle(UnassignUserRoleCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Unassigning user role: {@Request}", request);

            var user = await userManager.FindByEmailAsync(request.UserEmail);

            if (user == null)
            {
                throw new NotFoundException(nameof(user), request.UserEmail);
            }

            var role = await roleManager.FindByNameAsync(request.RoleName);

            if (role == null)
            {
                throw new NotFoundException(nameof(role), request.RoleName);
            }

            await userManager.RemoveFromRoleAsync(user, role.Name!);
        }
    }
}
