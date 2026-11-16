using BL.Dashboard;
using BL.Report;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text;

namespace UNICEF_App.Controllers
{    
    public class AdminReportController : Controller
    {
        private readonly ILogger<AdminReportController> _logger;
        private readonly IDepartmentReportService _iDepartmentReportService;
        private readonly IDashboard _iDashboard;
        public AdminReportController(ILogger<AdminReportController> logger, IDepartmentReportService iDepartmentReportService, IDashboard iDashboard)
        {
            _logger = logger;
            _iDepartmentReportService = iDepartmentReportService;
            _iDashboard = iDashboard;
        }

        #region Department Report by Department Login
        public IActionResult Department()
        {            
            var groupIdClaim =Convert.ToInt32(User.FindFirst("GroupId")?.Value);
            var deptIdClaim = 0;            
            if (groupIdClaim == 4) // DES
            {
                deptIdClaim = -1;
            }            
            else if (groupIdClaim == 2) // DEPARTMENT
            {
                deptIdClaim = Convert.ToInt32(User.FindFirst("DepartmentId")?.Value);
            }
            ViewBag.DeptId = deptIdClaim.ToString();
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
        #region Agency Report by Department Login
        public IActionResult AgencyByDepartment()
        {
            var groupIdClaim = Convert.ToInt32(User.FindFirst("GroupId")?.Value);
            var deptIdClaim = 0;
            if (groupIdClaim == 4) // DES
            {
                deptIdClaim = -1;
            }
            else if (groupIdClaim == 2) // DEPARTMENT
            {
                deptIdClaim = Convert.ToInt32(User.FindFirst("DepartmentId")?.Value);
            }
            ViewBag.DeptId = deptIdClaim.ToString();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetAgencyByDepartmentReportData(int? departmentId, string activityGuid = null)
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
        public IActionResult Sector()
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
        public IActionResult Agency()
        {
            var groupIdClaim = Convert.ToInt32(User.FindFirst("GroupId"));
            var deptIdClaim = 0;
            var agencyIdClaim = 0;
            if (groupIdClaim == 4) // DES
            {
                ViewBag.AgencyId = "-1";
            }
            else if (groupIdClaim == 2) // AGENCY
            {
                agencyIdClaim = Convert.ToInt32(User.FindFirst("AgencyId"));
                ViewBag.AgencyId = agencyIdClaim;
            }            
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetAgencyWiseReportData(int? agencyId, string activityGuid = null)
        {
            try
            {
                var groupIdClaim = Convert.ToInt32(User.FindFirst("GroupId"));
                var deptIdClaim = 0;
                var agencyIdClaim = 0;
                string jsonResult = string.Empty;
                if(groupIdClaim == 4)
                {

                }
                //else if ()
                //{

                //}
                    // 1. Activities ka raw JSON string get karein
                jsonResult = await _iDepartmentReportService.GetAgencyWiseReportDataAsync((int)agencyId, activityGuid);

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

        #region Department Report Action
        [HttpGet]
        public async Task<IActionResult> ReportAnalize(string? agencyId,string? sectorId, string? activityGuid)
        {
            try
            {
                var departmentId = User.FindFirst("DepartmentId")?.Value;
                //if (string.IsNullOrEmpty(userId))
                //{
                //    userId = User.FindFirst("DepartmentId")?.Value ?? User.FindFirst("DeptId")?.Value;
                //}
                string finalData = await _iDepartmentReportService.ReportAnalize(departmentId, agencyId, sectorId, activityGuid);

                return Content(finalData, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
        #endregion


        #region Agency Login Report
        public IActionResult AgencyWiseReport()
        {
            var agencyIdClaim = Convert.ToInt32(User.FindFirst("AgencyId")?.Value);
            ViewBag.AgencyId = agencyIdClaim;
            return View();
        }
        public async Task<IActionResult> GetAgencyLoginReport(int agencyId,string? deptId,string? sectorId, string? activityGuid = null)
        {
            try
            {                
                // 1. Activities ka raw JSON string get karein
                var jsonResult = await _iDepartmentReportService.GetAgencyLoginReport(agencyId,deptId,sectorId, activityGuid);

                // 2. Dashboard Service se Summary Counts fetch karein
                object summaryCounts = null;
                if (agencyId != null)
                {
                    summaryCounts = await _iDepartmentReportService.GetCountByAgencySectorDeptByAgency(agencyId,deptId,sectorId);
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
        #region Department Login Report
        public IActionResult DepartmentWiseReport()
        {
            var departmentIdClaim = Convert.ToInt32(User.FindFirst("DepartmentId")?.Value);
            ViewBag.DepartmentId = departmentIdClaim;
            return View();
        }
        public async Task<IActionResult> GetDepartmentLoginReport(int deptId, string? agencyId, string? sectorId, string? activityGuid = null)
        {
            try
            {
                // 1. Activities ka raw JSON string get karein
                var jsonResult = await _iDepartmentReportService.GetDepartmentLoginReport(deptId,agencyId, sectorId, activityGuid);

                // 2. Dashboard Service se Summary Counts fetch karein
                object summaryCounts = null;
                if (agencyId != null)
                {
                    summaryCounts = await _iDepartmentReportService.GetCountByAgencySectorDeptByDept(deptId,agencyId, sectorId);
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
