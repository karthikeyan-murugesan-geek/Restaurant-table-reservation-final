using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReservationService.Filters;
using System.Security.Claims;
using Xunit;
using Microsoft.AspNetCore.Routing;

namespace ReservationService.UnitTest
{
	public class CustomAuthorizationFilterTests
	{
        [Fact]
        public void Authorization_ShouldFail_WhenRoleNotMatching()
        {
            // Arrange
            var filter = new CustomAuthorizationFilter("Customer");

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Role, "Manager")
            }, "mock"));

            var context = new AuthorizationFilterContext(
                new ActionContext
                    {
                        HttpContext = new DefaultHttpContext { User = user },
                        RouteData = new RouteData(),  // <-- fix here
                        ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
                    },
                    new List<IFilterMetadata>()
                );

            // Act
            filter.OnAuthorization(context);

            // Assert
            Assert.IsType<JsonResult>(context.Result);
            Assert.Equal((context.Result as JsonResult).StatusCode, 403);
        }

        [Fact]
        public void Authorization_ShouldPass_WhenRoleMatches()
        {
            // Arrange
            var filter = new CustomAuthorizationFilter("Customer");

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Role, "Customer")
            }, "mock"));

            var context = new AuthorizationFilterContext(
                new ActionContext
                {
                    HttpContext = new DefaultHttpContext { User = user },
                    RouteData = new RouteData(),  // <-- fix here
                    ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
                },
                new List<IFilterMetadata>()
            );

            // Act
            filter.OnAuthorization(context);

            // Assert
            Assert.Null(context.Result);
        }
    }
}

