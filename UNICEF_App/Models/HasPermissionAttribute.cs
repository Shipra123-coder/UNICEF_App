using BL.Services.Permission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MO.Common;
using System.Security.Claims;

namespace UNICEF_App.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class HasPermissionAttribute : Attribute, IAsyncActionFilter
    {
        private readonly PermissionAction _actionType;
        private readonly string? _overrideController;
        private readonly string? _overrideAction;

        public HasPermissionAttribute(PermissionAction actionType, string? overrideController = null, string? overrideAction = null)
        {
            _actionType = actionType;
            _overrideController = overrideController;
            _overrideAction = overrideAction;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var user = httpContext.User;

            if (user == null || !user.Identity!.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            // User Group claim nikalna
            var groupIdClaim = user.FindFirst("GroupId")?.Value ?? user.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(groupIdClaim) || !int.TryParse(groupIdClaim, out int groupId))
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
                return;
            }

            string controllerName = _overrideController
                ?? context.RouteData.Values["controller"]?.ToString()
                ?? string.Empty;

            string actionName = _overrideAction
                ?? context.RouteData.Values["action"]?.ToString()
                ?? string.Empty;

            // BL resolve karein
            var permissionBL = httpContext.RequestServices.GetRequiredService<IPermissionBL>();

            bool isAuthorized = await permissionBL.HasAccessAsync(groupId, controllerName, actionName, _actionType);

            if (!isAuthorized)
            {
                // Ajax request handling
                if (httpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    context.Result = new JsonResult(new { success = false, message = "Access Denied: You do not have permission for this action." })
                    {
                        StatusCode = 403
                    };
                    return;
                }

                context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
                return;
            }

            await next();
        }
    }
}
