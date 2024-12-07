using Auth.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Auth.Core.Helpers
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class Authorization : Attribute, IAuthorizationFilter
    {
        private readonly IList<UserRole> _roles;
        public Authorization(params UserRole[] _roles)
        {
            _roles = _roles ?? new UserRole[] { };
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isRolePermission = false;
            User user = (User)context.HttpContext.Items["User"];
            if (user == null) 
            {
                context.Result = new JsonResult(new { Message = "Unauthorization" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }

            if (user != null && _roles.Any())
            {
                foreach (var userRole in user.UserRoles)
                {
                    foreach (var authRole in _roles)
                    {
                        if (userRole == authRole)
                        {
                            isRolePermission = true;
                        }
                    }
                }

                if (!isRolePermission)
                {
                    context.Result = new JsonResult(new { Message = "Unauthorization" }) { StatusCode = StatusCodes.Status401Unauthorized };
                }
            }
        }
    }
}
