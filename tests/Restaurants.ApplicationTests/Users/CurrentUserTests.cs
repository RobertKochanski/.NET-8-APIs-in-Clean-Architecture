using FluentAssertions;
using Restaurants.Domain.Contants;
using Xunit;

namespace Restaurants.Application.Users.Tests
{
    public class CurrentUserTests
    {
        // TestMethod_Scenario_ExpectResult
        [Theory()]
        [InlineData(UserRoles.Admin)]
        [InlineData(UserRoles.User)]
        public void IsInRole_WithMathingRole_ShouldReturnTrue(string roleName)
        {
            // arrange
            var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

            // arc
            var result = currentUser.IsInRole(roleName);

            // assert

            result.Should().BeTrue();
        }

        [Fact()]
        public void IsInRole_WithoMathingRole_ShouldReturnFalse()
        {
            // arrange
            var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

            // arc
            var result = currentUser.IsInRole(UserRoles.Owner);

            // assert

            result.Should().BeFalse();
        }

        [Fact()]
        public void IsInRole_WithoMathingRoleCase_ShouldReturnFalse()
        {
            // arrange
            var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

            // arc
            var result = currentUser.IsInRole(UserRoles.Admin.ToLower());

            // assert

            result.Should().BeFalse();
        }
    }
}