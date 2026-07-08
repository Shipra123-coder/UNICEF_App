using BL.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MO.Master;
using MO.ProfileUser;

namespace UNICEF_App.Controllers
{
    public class MasterController : Controller
    {
        private readonly ICommon _iCommon;
        public MasterController(ICommon iCommon)
        {
          _iCommon = iCommon;
        }
        public IActionResult Agency()
        {
            AgencyMasterMO model = new AgencyMasterMO();
            return View(model);
        }
        public IActionResult Department()
        {
            DepartmentMasterMO model = new DepartmentMasterMO();
            return View(model);
        }
        public IActionResult ActivityMaster(int? id)
        {
            id = 1;
            var model = new WorkActivityViewModel
            {
                Activity = new WorkActivityMO
                {
                    ActivityId = (int)id,
                    ActivityName = "Health Awareness Camp"
                },

                DepartmentList = new List<DepartmentMasterMO>
                {
                    new DepartmentMasterMO { Id = 1, DepartmentName = "Health Department" },
                    new DepartmentMasterMO { Id = 2, DepartmentName = "Education Department" },
                    new DepartmentMasterMO { Id = 3, DepartmentName = "Agriculture Department" },
                    new DepartmentMasterMO { Id = 4, DepartmentName = "IT Department" }
                },

                // Example selected
                SelectedDepartmentIds = new List<int> { 1, 2, 3 },
                NodalDepartmentId = 1,

                AgencyList = new List<AgencyMasterMO>
                {
                    new AgencyMasterMO { Id = 1, AgencyName = "United Nations Development Programme" },
                    new AgencyMasterMO { Id = 2, AgencyName = "United Nations Children's Fund" },
                    new AgencyMasterMO { Id = 3, AgencyName = "World Health Organization" },
                    new AgencyMasterMO { Id = 4, AgencyName = "Food and Agriculture Organization" }
                },

                // Example selected
                SelectedAgencyIds = new List<int> { 1, 2, 3 },
                NodalAgencyId = 1
            };

            return View(model);
        }
        public IActionResult UserMaster()
        {
            var model = new UserMasterMO
            {
                // 🔹 Department List
                departmentListDDLs = new List<DepartmentListDDL>
              {
                  new DepartmentListDDL { Id = 1, DepartmentName = "Health Department" },
                  new DepartmentListDDL { Id = 2, DepartmentName = "Education Department" },
                  new DepartmentListDDL { Id = 3, DepartmentName = "Agriculture Department" },
                  new DepartmentListDDL { Id = 4, DepartmentName = "IT Department" }
              },
              
                      // 🔹 Agency List
                      agencyListDDLs = new List<AgencyListDDL>
              {
                  new AgencyListDDL { Id = 1, AgencyName = "UNDP" },
                  new AgencyListDDL { Id = 2, AgencyName = "UNICEF" },
                  new AgencyListDDL { Id = 3, AgencyName = "WHO" },
                  new AgencyListDDL { Id = 4, AgencyName = "FAO" }
              },
              
                      // 🔹 Group List
                      groupListDDLs = new List<GroupDDL>
              {
                  new GroupDDL { Id =1, Name = "Admin Group" },
                  new GroupDDL { Id =2, Name = "Department Group" },
                  new GroupDDL { Id =3, Name = "Agency Group" }
              },
              
                      // 🔹 User Level
                      userLevelListDDLs = new List<UserLevelListDDL>
              {
                  new UserLevelListDDL { Id =1, Name = "Department Level" },
                  new UserLevelListDDL { Id =2, Name = "Agency Level" },
                  new UserLevelListDDL { Id =3, Name = "DES Level" }
              }
            };
            return View(model);
        }

        #region BindDropDown
        [Route("Master/DDL_Agency")]
        public async Task<IActionResult> DDL_Agency()
        {
            //if (!User.Identity.IsAuthenticated)
            //{
            //    // Handle the case where the user is not authenticated
            //    return Unauthorized(); // Returns a 401 Unauthorized response
            //}
            return Json(await _iCommon.DDL_AgencyAsync());
        }
        [Route("Master/DDL_Department")]
        public async Task<IActionResult> DDL_Department()
        {
            //if (!User.Identity.IsAuthenticated)
            //{
            //    // Handle the case where the user is not authenticated
            //    return Unauthorized(); // Returns a 401 Unauthorized response
            //}
            return Json(await _iCommon.DDL_DepartmentAsync());
        }
        [Route("Master/DDL_SupDepartment")]
        public async Task<IActionResult> DDL_SupDepartment(int? Id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Handle the case where the user is not authenticated
                return Unauthorized(); // Returns a 401 Unauthorized response
            }
            return Json(await _iCommon.DDL_SupDepartmentAsync(Id));
        }

        [Route("Master/DDL_Pillar")]
        public async Task<IActionResult> DDL_Pillar()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Handle the case where the user is not authenticated
                return Unauthorized(); // Returns a 401 Unauthorized response
            }
            return Json(await _iCommon.DDL_PillerAsync());
        }

        [Route("Master/DDL_Sector")]
        public async Task<IActionResult> DDL_Sector(int Id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Handle the case where the user is not authenticated
                return Unauthorized(); // Returns a 401 Unauthorized response
            }
            return Json(await _iCommon.DDL_SectorAsync(Id));
        }
        [Route("Master/DDL_SubSector")]
        public async Task<IActionResult> DDL_SubSector(int Id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Handle the case where the user is not authenticated
                return Unauthorized(); // Returns a 401 Unauthorized response
            }
            return Json(await _iCommon.DDL_SubSectorAsync(Id));
        }
        
        [Route("Master/DDL_Goal")]
        public async Task<IActionResult> DDL_Goal()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Handle the case where the user is not authenticated
                return Unauthorized(); // Returns a 401 Unauthorized response
            }
            return Json(await _iCommon.DDL_GoalAsync());
        }

        [Route("Master/DDL_Target")]
        public async Task<IActionResult> DDL_Target(int? GoalId)
        {
            return Json(await _iCommon.DDL_TargetAsync(GoalId));
        }

        [Route("Master/DDL_District")]
        public async Task<IActionResult> DDL_District()
        {
            return Json(await _iCommon.DDL_DistrictAsync());
        }
        [Route("Master/DDL_Block")]
        public async Task<IActionResult> DDL_Block(int? DistrictId)
        {
            return Json(await _iCommon.DDL_BlockAsync(DistrictId));
        }
        [Route("Master/DDL_City")]
        public async Task<IActionResult> DDL_City(int? DistrictId)
        {
            return Json(await _iCommon.DDL_CityAsync(DistrictId));
        }

        [Route("Master/DDL_UNSector")]
        public async Task<IActionResult> DDL_UNSector()
        {
            //if (!User.Identity.IsAuthenticated)
            //{
            //    // Handle the case where the user is not authenticated
            //    return Unauthorized(); // Returns a 401 Unauthorized response
            //}
            return Json(await _iCommon.DDL_UNSectorAsync());
        }

        [Route("Master/DDL_NatureOfSupport")]
        public async Task<IActionResult> DDL_NatureOfSupport()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Handle the case where the user is not authenticated
                return Unauthorized(); // Returns a 401 Unauthorized response
            }
            return Json(await _iCommon.DDL_NatureOfSupportAsync());
        }

        [Route("Master/DDL_SubNatureOfSupport")]
        public async Task<IActionResult> DDL_SubNatureOfSupport(int? NatureSupportId)
        {
            return Json(await _iCommon.DDL_SubNatureOfSupportAsync(NatureSupportId));
        }
        #endregion
    }
}
