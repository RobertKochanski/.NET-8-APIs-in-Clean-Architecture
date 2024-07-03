using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Domain.Contants;
using System.Security.Claims;
using Xunit;

namespace Restaurants.Application.Users.Tests
{
    public class UserContextTests
    {
        [Fact()]
        public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
        {
            // arrange
            var dateOfBirth = new DateOnly(1990, 1, 20);
            var id = Guid.NewGuid().ToString();

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Email, "test@test.com"),
                new Claim(ClaimTypes.Role, UserRoles.Admin),
                new Claim(ClaimTypes.Role, UserRoles.User),
                new Claim("Nationality", "German"),
                new Claim("DateOfBirth", dateOfBirth.ToString("yyyy-MM-dd")),
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "test"));

            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext()
            {
                User = user
            });

            var userContext = new UserContext(httpContextAccessorMock.Object);

            // act

            var currentUser = userContext.GetCurrentUser();

            // assert

            currentUser.Should().NotBeNull();
            currentUser.Id.Should().Be(id);
            currentUser.Email.Should().Be("test@test.com");
            currentUser.Roles.Should().ContainInOrder(UserRoles.Admin, UserRoles.User);
            currentUser.Nationality.Should().Be("German");
            currentUser.DateOfBirth.Should().Be(dateOfBirth);
        }

        [Fact()]
        public void GetCurrentUser_WithUserContextNotPresent_ThrowsInvalidOperationException()
        {
            // arrange
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);

            var userContext = new UserContext(httpContextAccessorMock.Object);

            // act
            Action action = () => userContext.GetCurrentUser();

            // assert
            action.Should().Throw<InvalidOperationException>().WithMessage("User context is not present");
        }
    }
}