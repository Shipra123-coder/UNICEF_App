using BL.AI;
using Microsoft.AspNetCore.Mvc;

namespace UNICEF_App.Controllers
{
    public class AIController : Controller
    {
        private readonly IOpenAIService _service;
        public AIController(IOpenAIService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> Ask(string question)
        {
            var result = await _service.AskAI(question);

            return Content(result);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
