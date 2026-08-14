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

        #region By Department Report
        public IActionResult DepartmentReport()
        {
            ViewBag.DeptId = "-1";
            return View();
        }
       
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

        #endregion

        #region By Sector Report
        public IActionResult SectorReport()
        {            
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetSectorWiseReportData(int? sectorId, string activityGuid = null)
        {
            try
            {
                // 1. Activities ka raw JSON string get karein
                string jsonResult = await _iDepartmentReportService.GetSectorWiseReportDataAsync(sectorId, activityGuid);

                // 2. Dashboard Service se Summary Counts fetch karein
                object summaryCounts = null;
                if (sectorId.HasValue && sectorId.Value > 0)
                {
                    summaryCounts = await _iDashboard.GetDashboardCountBySector(sectorId.Value);
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
        #endregion

        #region By Agency Report
        public IActionResult AgencyReport()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetAgencyWiseReportData(int? agencyId, string activityGuid = null)
        {
            try
            {
                // 1. Activities ka raw JSON string get karein
                string jsonResult = await _iDepartmentReportService.GetAgencyWiseReportDataAsync(agencyId, activityGuid);

                // 2. Dashboard Service se Summary Counts fetch karein
                object summaryCounts = null;
                if (agencyId.HasValue && agencyId.Value > 0)
                {
                    summaryCounts = await _iDashboard.GetDashboardCountByAgency(agencyId.Value);
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
        #endregion
    }
}
