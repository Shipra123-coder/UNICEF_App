using BL.Dashboard;
using Microsoft.AspNetCore.Mvc;
using MO.DashBoard;
using NuGet.Protocol.Core.Types;
using System.Diagnostics;
using System.Threading.Tasks;
using UNICEF_App.Models;

namespace UNICEF_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDashboard _iDashboard;
        public HomeController(ILogger<HomeController> logger,IDashboard iDashboard)
        {
            _logger = logger;
            _iDashboard =iDashboard;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View(); 
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Home()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult Land()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCoverPages()
        {
            var data = await _iDashboard.GetCoverPages();

            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetCoverPagesBySector(string sectorId)
        {
            var data = await _iDashboard.GetCoverPagesBySector(sectorId);
            return Json(data);
        }

        public async Task<IActionResult> BestPrectices()
        {

            var model = await _iDashboard.GetSectorWiseCount();
            return View(model);
            
        }
        public async Task<IActionResult> BestPrecticesSubSector(int sectorId)
        {
            var model = await _iDashboard.GetDashboardCountBySector(sectorId);

            //model.SectorId = sectorId;
            return View(model);

        }
        [HttpGet]
        public async Task<IActionResult> GetCountByAgencySectorDept(int? sectorId,int? agencyId,int? departmentId)
        {
            var model = await _iDashboard.GetCountByAgencySectorDept(
                            sectorId,
                            agencyId,
                            departmentId);

            return Json(model);
        }

        #region Agencys
        public IActionResult Agency()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAgencyList()
        {
            var data = await _iDashboard.GetAgencyList();
            return Json(data);
        }

        public async Task<IActionResult> AgencyStatus(int agencyId)
        {
            var model = await _iDashboard.GetDashboardCountByAgency(agencyId);
            //model.SectorId = sectorId;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetCoverPagesByAgency(string agencyId)
        {
            var data = await _iDashboard.GetCoverPagesByAgency(agencyId);
            return Json(data);
        }

        #endregion

        #region Department
        public IActionResult Department()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetDepartmentList()
        {
            var data = await _iDashboard.GetDepartmentList();
            return Json(data);
        }
        public async Task<IActionResult> DepartmentStatus(int deptId)
        {
            var model = await _iDashboard.GetDashboardCountByDept(deptId);

            //model.SectorId = sectorId;
            return View(model);

        }
        #endregion

        public IActionResult ActivityDet()
        {
            return View();
        }

        #region Agency
        [HttpGet]
        public async Task<IActionResult>GetAgencyDepartmentActivityList(int? agencyId)
        {
            var data = await _iDashboard.GetAgencyDepartmentActivityList(agencyId);
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult>GetAgencySectorActivityList(int? agencyId)
        {
            var data = await _iDashboard.GetAgencySectorActivityList(agencyId);
            return Json(data);
        }
        #endregion

        #region Sector

        [HttpGet]
        public async Task<IActionResult> GetSectorDepartmentActivityList(int? sectorId)
        {
            var data = await _iDashboard.GetSectorDepartmentActivityList(sectorId);
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult>GetSectorAgencyActivityList(int? sectorId)
        {
            var data = await _iDashboard.GetSectorAgencyActivityList(sectorId);
            return Json(data);
        }
        #endregion

        #region Department

        [HttpGet]
        public async Task<IActionResult>GetDepartmentAgencyActivityList(int? departmentId)
        {
            var data = await _iDashboard.GetDepartmentAgencyActivityList(departmentId);
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult>GetDepartmentSectorActivityList(int? departmentId)
        {
            var data = await _iDashboard.GetDepartmentSectorActivityList(departmentId);
            return Json(data);
        }
        #endregion

        #region Chart
        [HttpGet]
        public async Task<IActionResult> GetAgencyChartData()
        {
            var result =
                await _iDashboard.GetAgencyChartData();

            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetDepartmentChartData()
        {
            var result =
                await _iDashboard.GetDepartmentChartData();

            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetGoalChartData()
        {
            var result =
                await _iDashboard.GetGoalChartData();

            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetActivityStatusChart()
        {
            var data =
                await _iDashboard.GetActivityStatusChartData();

            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetTaskStatusChart()
        {
            var result = await _iDashboard.GetTaskStatusChartData();

            return Json(result);
        }

        #endregion

        #region ContactUs
        public IActionResult ContactUs()
        {
            return View();
        }
        #endregion

    }
}
