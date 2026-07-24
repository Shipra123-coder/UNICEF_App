using BL.ViksitService;
using Microsoft.AspNetCore.Mvc;

namespace UNICEF_App.Areas.Public.Controllers
{
    [Area("Public")]
    public class ViksitRajasthanController : Controller
    {
        private readonly ILogger<ViksitRajasthanController> _logger;
        private readonly IViksitService _iViksitService;
        public ViksitRajasthanController(ILogger<ViksitRajasthanController> logger, IViksitService iViksitService)
        {
            _logger = logger;
            _iViksitService = iViksitService;
        }

        #region Viksit Rajasthan
        public async Task<IActionResult> ViksitRajasthan()
        {
            var model = await _iViksitService.GetAllPillarsWithActivityCountAsync();
            return View(model);
        }
        public async Task<IActionResult> ViksitRajasthanProgress(int viksitId)
        {
            //var model = await _iViksitRajasthan.GetDashboardCountBySector(viksitId);           
            //return View(model);

            return View();

        }
        #endregion
    }
}
