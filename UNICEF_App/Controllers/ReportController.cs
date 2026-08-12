using BL.Dashboard;
using BL.Report;
using Microsoft.AspNetCore.Mvc;
using MO.Management;
using Newtonsoft.Json;

namespace UNICEF_App.Controllers
{
    public class ReportController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDepartmentReportService _iDepartmentReportService;
        private readonly IDashboard _iDashboard;
        public ReportController(ILogger<HomeController> logger, IDepartmentReportService iDepartmentReportService, IDashboard iDashboard)
        {
            _logger = logger;
            _iDepartmentReportService = iDepartmentReportService;
            _iDashboard = iDashboard;
        }
        public IActionResult DepartmentReport()
        {
            ViewBag.DeptId = 5;
            return View();
        }

        //[HttpGet]
        //public async Task<IActionResult> GetDepartmentWiseReportData(int? departmentId, string activityGuid = null)
        //{
        //    try
        //    {
        //        string jsonResult = await _iDepartmentReportService.GetDepartmentWiseReportDataAsync(departmentId, activityGuid);

        //        // Raw JSON Content Return (Front-end direct parse kar sakega)
        //        return Content(jsonResult, "application/json");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { success = false, message = ex.Message });
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> GetDepartmentWiseReportData(int? departmentId, string activityGuid = null)
        {
            try
            {
                // 1. Activities ka raw JSON string get karein
                string jsonResult = await _iDepartmentReportService.GetDepartmentWiseReportDataAsync(departmentId, activityGuid);

                // 2. Dashboard Service se Summary Counts fetch karein
                object summaryCounts = null;
                if (departmentId.HasValue && departmentId.Value > 0)
                {
                    summaryCounts = await _iDashboard.GetDashboardCountByDept(departmentId.Value);
                }

                // 3. Raw jsonResult ko object me deserialize karein taaki combine karte waqt string escape (\") na ho
                var activitiesData = System.Text.Json.JsonSerializer.Deserialize<object>(jsonResult);

                // 4. Activities aur Summary counts ko combine object me return karein
                var response = new
                {
                    activities = activitiesData,
                    summary = summaryCounts
                };

                return Json(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}
