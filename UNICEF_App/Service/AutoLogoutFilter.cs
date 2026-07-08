using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UNICEF_App.Service
{
    public class AutoLogoutFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated ||
                user.FindFirst("UserId") == null)
            {
                context.HttpContext.SignOutAsync("Cookies");
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
