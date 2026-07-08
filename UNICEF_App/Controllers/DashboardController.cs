using Microsoft.AspNetCore.Mvc;

namespace UNICEF_App.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
