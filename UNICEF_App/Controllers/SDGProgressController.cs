using BL.SDGGoalService;
using BL.ViksitService;
using Microsoft.AspNetCore.Mvc;

namespace UNICEF_App.Controllers
{
    public class SDGProgressController : Controller
    {
        private readonly ILogger<SDGProgressController> _logger;
        private readonly ISDGGoalServices _iSDGGoalService;
        public SDGProgressController(ILogger<SDGProgressController> logger, ISDGGoalServices iSDGGoalService)
        {
            _logger = logger;
            _iSDGGoalService = iSDGGoalService;
        }

        #region SDGs Goals Progress
        public async Task<IActionResult> SDGGoal()
        {
            var model = await _iSDGGoalService.GetAllPillarsWithActivityCountAsync();
            return View(model);
        }
        public async Task<IActionResult> SDGGoalProgress(int goalId)
        {
            var model = await _iSDGGoalService.GetGoalWiseCounts(goalId);
            return View(model);           
        }

        [HttpGet]
        public async Task<IActionResult> GetCoverPagesByGoals(string goalId)
        {
            //var data = await _iDashboard.GetCoverPagesByAgency(viksitId);
            //return Json(data);
            return Json(new { success = true, message = "Data fetched successfully", data = new { } });
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartmentActivityList(int? goalId)
        {
            var data = await _iSDGGoalService.GetDepartmentActivityList(goalId);
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetSectorActivityList(int? goalId)
        {
            var data = await _iSDGGoalService.GetSectorActivityList(goalId);
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAgencyActivityList(int? goalId)
        {
            var data = await _iSDGGoalService.GetAgencyActivityList(goalId);
            return Json(data);
        }
        #endregion
    }
}
