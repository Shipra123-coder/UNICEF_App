using Microsoft.AspNetCore.Mvc;

namespace UNICEF_App.Controllers
{
    public class ErrorController : Controller
    {
        [Route("StatusError/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 403:
                    //ViewBag.ErrorMessage = "Sorry, you are not authorized to access this page.";
                    return View("Forbidden", "Sorry, you are not authorized to access this page."); // 403 Forbidden
                case 404:
                    //ViewBag.ErrorMessage = "Sorry, the page you requested could not be found.";
                    return View("NotFound", "Sorry, the page you requested could not be found."); // 404 Not Found
                default:
                    //ViewBag.ErrorMessage = "An unexpected error occurred.";
                    return View("Error", "An unexpected error occurred."); // General error page
            }
        }

        public IActionResult AccessDenied()
        {
            ViewBag.Message = "You do not have permission to access this page.";
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
