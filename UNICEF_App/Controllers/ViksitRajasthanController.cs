using BL.ViksitService;
using Microsoft.AspNetCore.Mvc;

namespace UNICEF_App.Controllers
{
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
            var model = await _iViksitService.GetPillarWiseCounts(viksitId);
            return View(model);

            //return View();

        }

        [HttpGet]
        public async Task<IActionResult> GetCoverPagesByViksit(string viksitId)
        {
            //var data = await _iDashboard.GetCoverPagesByAgency(viksitId);
            //return Json(data);
            return Json(new { success = true, message = "Data fetched successfully", data = new { } });
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartmentActivityList(int? viksitId)
        {
            var data = await _iViksitService.GetDepartmentActivityList(viksitId);
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetSectorActivityList(int? viksitId)
        {
            var data = await _iViksitService.GetSectorActivityList(viksitId);
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAgencyActivityList(int? viksitId)
        {
            var data = await _iViksitService.GetAgencyActivityList(viksitId);
            return Json(data);
        }
        #endregion
    }
}
