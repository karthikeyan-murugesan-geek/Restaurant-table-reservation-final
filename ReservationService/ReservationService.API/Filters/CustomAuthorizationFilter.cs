using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ReservationService.Filters
{
    public class CustomAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles = new string[] { "Manager", "Customer" };

        public CustomAuthorizationFilter(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity?.IsAuthenticated ?? true)
            {
                context.Result = new JsonResult(new { message = "Authentication required" })
                { StatusCode = 401 };
                return;
            }

            if (!_roles.Any(user.IsInRole))
            {
                context.Result = new JsonResult(new { message = "You are not authorized" })
                { StatusCode = 403 };
            }
        }
    }
}

