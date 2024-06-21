using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.AssignUserRole
{
    internal class AssignUserRoleCommandHandler(ILogger<AssignUserRoleCommandHandler> logger,
        UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager) : IRequestHandler<AssignUserRoleCommand>
    {
        public async Task Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Assigning user role: {@Request}", request);

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

            await userManager.AddToRoleAsync(user, role.Name!);
        }
    }
}
