using BL.Common;
using BL.Services.Agency;
using BL.Services.CMDetailsBL;
using BL.Services.ContactBL;
using BL.Services.Department;
using BL.Services.Group;
using BL.Services.MenuBL;
using BL.Services.MenuPermission;
using BL.Services.NatureOfSupportBL;
using BL.Services.NodalDetail;
using BL.Services.SubNatureOfSupportBL;
using BL.Services.SubThemes;
using BL.Services.Theme;
using BL.Services.UNSector;
using BL.Services.UserLevel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MO.Entities;
using MO.Master;
using MO.ProfileUser;
using MO.ViewModels;


namespace UNICEF_App.Controllers
{
    public class MasterController : Controller
    {
        private readonly ICommon _iCommon;
        private readonly IUserLevelBL _iUserLevelBL;
        private readonly IDepartmentBL _iDepartmentBL;
        private readonly IAgencyBL _iAgencyBL;
        private readonly IThemeBL _iThemeBL;
        private readonly ISubThemesBL _iSubThemesBL;
        private readonly IUNSectorBL _iUNSectorBL;
        private readonly IGroupBL _iGroupBL;
        private readonly INatureOfSupportBL _iNatureOfSupportBL;
        private readonly ISubNatureOfSupportBL _iSubNatureOfSupportBL;
        private readonly IMenuBL _iMenuBL;
        private readonly IContactBL _iContactBL;
        private readonly ICMDetailsBL _iCMDetailsBL;
        private readonly INodalDetailBL _iNodalDetailBL;
        private readonly IMenuPermissionBL _iMenuPermissionBL;
        private readonly IWebHostEnvironment _env;
        public MasterController(ICommon iCommon, IUserLevelBL iUserLevel
            , IDepartmentBL iDepartmentBL, IAgencyBL iAgencyBL
            , IThemeBL iThemeBL
            , ISubThemesBL iSubThemes
            , IUNSectorBL iUNSectorBL
            , IGroupBL iGroupBL
            ,INatureOfSupportBL iNatureOfSupportBL
            , ISubNatureOfSupportBL iSubNatureOfSupportBL
            ,IMenuBL iMenuBL
            , IContactBL iContactBL
            ,ICMDetailsBL iCMDetailsBL
            ,INodalDetailBL iNodalDetailBL
            , IMenuPermissionBL iMenuPermissionBL
            , IWebHostEnvironment env)
        {
            _iCommon = iCommon;
            _iUserLevelBL = iUserLevel;
            _iDepartmentBL = iDepartmentBL;
            _iAgencyBL = iAgencyBL;
            _iThemeBL = iThemeBL;
            _iSubThemesBL = iSubThemes;
            _iUNSectorBL= iUNSectorBL;
            _iGroupBL= iGroupBL;
            _iNatureOfSupportBL = iNatureOfSupportBL;
            _iSubNatureOfSupportBL = iSubNatureOfSupportBL;
            _iMenuBL = iMenuBL;
            _iContactBL = iContactBL;
            _iCMDetailsBL = iCMDetailsBL;
            _iNodalDetailBL = iNodalDetailBL;
            _iMenuPermissionBL = iMenuPermissionBL;
            _env = env;
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

        #region UserLevel Master
        [HttpGet]
        public async Task<IActionResult> UserLevel(int? id)
        {
            var data = await _iUserLevelBL.GetAllUserLevelsAsync();

            var viewModel = new UserLevelViewModel
            {
                // LINQ Select se Entity ko Model me map karein
                UserLevelList = data.Select(x => new UserLevelModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    CreatedDate = x.CreatedDate
                }).ToList()
            };

            if (id.HasValue && id.Value > 0)
            {
                var existing = await _iUserLevelBL.GetUserLevelByIdAsync(id.Value);
                if (existing != null)
                {
                    viewModel.FormModel = new UserLevelModel
                    {
                        Id = existing.Id,
                        Name = existing.Name,
                        CreatedDate = existing.CreatedDate
                    };
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserLevelSave(UserLevelViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                // Entity ko ViewModel List mein map karein
                var data = await _iUserLevelBL.GetAllUserLevelsAsync();
                viewModel.UserLevelList = data.Select(x => new UserLevelModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    CreatedDate = x.CreatedDate
                }).ToList();

                return View("UserLevel", viewModel);
            }

            // FormModel ko Entity mein map karein agar Service Entity expect karti hai
            var entity = new m_UserLevel
            {
                Id = viewModel.FormModel.Id,
                Name = viewModel.FormModel.Name,
                CreatedDate = viewModel.FormModel.CreatedDate
            };

            var result = await _iUserLevelBL.SaveUserLevelAsync(entity);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;

            return RedirectToAction(nameof(UserLevel));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserLevelDelete(int id)
        {
            var result = await _iUserLevelBL.DeleteUserLevelAsync(id);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Department Master      
        [HttpGet]
        public async Task<IActionResult> Department(Guid? guid)
        {
            var viewModel = new DepartmentViewModel
            {
                DepartmentList = await _iDepartmentBL.GetAllDepartmentsAsync()
            };

            // Check karein ki guid null nahi hai aur empty nahi hai
            if (guid.HasValue && guid.Value != Guid.Empty)
            {
                // guid.Value pass karein jo ki non-nullable 'Guid' type hai
                var existing = await _iDepartmentBL.GetDepartmentByIdAsync(guid.Value);
                if (existing != null)
                {
                    viewModel.FormModel = existing;
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveDepartment(DepartmentViewModel viewModel)
        {
            // Agar user ne file upload nahi ki, toh ModelState se error remove karein (Optional banane ke liye)
            if (viewModel.FormModel.LogoFile == null || viewModel.FormModel.LogoFile.Length == 0)
            {
                ModelState.Remove("FormModel.LogoFile");
            }
            else
            {
                // Validation SIRF tab chalegi jab user ne koi file choose ki ho
                const long maxFileSize = 2 * 1024 * 1024; // 2 MB
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var extension = Path.GetExtension(viewModel.FormModel.LogoFile.FileName).ToLowerInvariant();

                if (viewModel.FormModel.LogoFile.Length > maxFileSize)
                {
                    ModelState.AddModelError("FormModel.LogoFile", "File size cannot exceed 2 MB.");
                }

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("FormModel.LogoFile", "Only JPG, JPEG, PNG, and WebP formats are allowed.");
                }
            }

            if (!ModelState.IsValid)
            {
                viewModel.DepartmentList = await _iDepartmentBL.GetAllDepartmentsAsync();
                return View("Department", viewModel);
            }

            // File Save Tabhi hogi jab nayi file aayi ho
            if (viewModel.FormModel.LogoFile != null && viewModel.FormModel.LogoFile.Length > 0)
            {
                string uploadFolder = Path.Combine(_env.WebRootPath, "uploads", "departments");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                // Agar edit mode mein nayi image choose ki, toh purani disk se delete karein
                if (!string.IsNullOrEmpty(viewModel.FormModel.LogoUrl))
                {
                    string oldPhysicalPath = Path.Combine(_env.WebRootPath, viewModel.FormModel.LogoUrl.TrimStart('/', '\\'));
                    if (System.IO.File.Exists(oldPhysicalPath))
                    {
                        try { System.IO.File.Delete(oldPhysicalPath); } catch { }
                    }
                }

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.FormModel.LogoFile.FileName);
                string newFilePath = Path.Combine(uploadFolder, uniqueFileName);

                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    await viewModel.FormModel.LogoFile.CopyToAsync(stream);
                }

                viewModel.FormModel.LogoUrl = "/uploads/departments/" + uniqueFileName;
            }
            // Agar file choose nahi ki, toh jo FormModel.LogoUrl (hidden field) mein purana path tha wahi maintain rahega

            var result = await _iDepartmentBL.SaveDepartmentAsync(viewModel.FormModel);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;

            return RedirectToAction(nameof(Department));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatusDepartment(Guid guid)
        {
            var result = await _iDepartmentBL.ToggleDepartmentStatusAsync(guid);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Department));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<(bool Success, string Message)> DeleteDepartmentAsync(Guid guid)
        {
            if (guid == Guid.Empty) return (false, "Invalid Department ID.");

            // 1. Department details fetch karein LogoUrl nikalne ke liye
            var department = await _iDepartmentBL.GetDepartmentByIdAsync(guid);
            if (department == null)
            {
                return (false, "Department not found.");
            }

            string logoUrl = department.LogoUrl;

            // 2. Database se delete karein
            var result = await _iDepartmentBL.DeleteDepartmentAsync(guid);

            if (result.Success)
            {
                // 3. Database se delete hone ke baad physical file delete karein
                if (!string.IsNullOrEmpty(logoUrl))
                {
                    try
                    {
                        // Relative URL (/uploads/departments/abc.jpg) ko physical path mein convert karein
                        string relativePath = logoUrl.TrimStart('/', '\\');
                        string physicalPath = Path.Combine(_env.WebRootPath, relativePath);

                        // Sahi (Full Namespace use karein):
                        if (System.IO.File.Exists(physicalPath))
                        {
                            System.IO.File.Delete(physicalPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        // File delete fail hone par bhi DB delete succeed rahega (logging kar sakte hain)
                    }
                }

                return (true, "Department and associated logo deleted successfully.");
            }

            return (false, "Failed to delete department.");
        }
        #endregion
        #region Agency
        [HttpGet]
        public async Task<IActionResult> Agency(Guid? guid)
        {
            var viewModel = new AgencyViewModel
            {
                AgencyList = await _iAgencyBL.GetAllAgenciesAsync()
            };

            if (guid.HasValue && guid.Value != Guid.Empty)
            {
                var existing = await _iAgencyBL.GetAgencyByGuidAsync(guid.Value);
                if (existing != null)
                {
                    viewModel.FormModel = existing;
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAgency(AgencyViewModel viewModel)
        {
            // Optional file validation only when file is selected
            if (viewModel.FormModel.LogoFile != null && viewModel.FormModel.LogoFile.Length > 0)
            {
                const long maxFileSize = 2 * 1024 * 1024; // 2 MB
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var extension = Path.GetExtension(viewModel.FormModel.LogoFile.FileName).ToLowerInvariant();

                if (viewModel.FormModel.LogoFile.Length > maxFileSize)
                {
                    ModelState.AddModelError("FormModel.LogoFile", "File size cannot exceed 2 MB.");
                }

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("FormModel.LogoFile", "Only JPG, JPEG, PNG, and WebP images are allowed.");
                }
            }
            else
            {
                ModelState.Remove("FormModel.LogoFile");
            }

            if (!ModelState.IsValid)
            {
                viewModel.AgencyList = await _iAgencyBL.GetAllAgenciesAsync();
                return View("Agency", viewModel);
            }

            // Save new logo file if provided
            if (viewModel.FormModel.LogoFile != null && viewModel.FormModel.LogoFile.Length > 0)
            {
                string uploadFolder = Path.Combine(_env.WebRootPath, "public", "img", "Agency_logo");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                // Delete old image if updating
                if (!string.IsNullOrEmpty(viewModel.FormModel.LogoURL))
                {
                    string oldRelativePath = viewModel.FormModel.LogoURL.TrimStart('/', '\\');
                    string oldPhysicalPath = Path.Combine(_env.WebRootPath, oldRelativePath);

                    if (System.IO.File.Exists(oldPhysicalPath))
                    {
                        try { System.IO.File.Delete(oldPhysicalPath); } catch { /* Ignore file lock/delete errors */ }
                    }
                }

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.FormModel.LogoFile.FileName);
                string newFilePath = Path.Combine(uploadFolder, uniqueFileName);

                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    await viewModel.FormModel.LogoFile.CopyToAsync(stream);
                }

                viewModel.FormModel.LogoURL = "/public/img/Agency_logo/" + uniqueFileName;
            }

            var result = await _iAgencyBL.SaveAgencyAsync(viewModel.FormModel);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;

            return RedirectToAction(nameof(Agency));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatusAgency(Guid guid)
        {
            var result = await _iAgencyBL.ToggleAgencyStatusAsync(guid);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Agency));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAgency(Guid guid)
        {
            var agency = await _iAgencyBL.GetAgencyByGuidAsync(guid);
            var result = await _iAgencyBL.DeleteAgencyAsync(guid);

            if (result.Success && agency != null && !string.IsNullOrEmpty(agency.LogoURL))
            {
                try
                {
                    string relativePath = agency.LogoURL.TrimStart('/', '\\');
                    string physicalPath = Path.Combine(_env.WebRootPath, relativePath);

                    if (System.IO.File.Exists(physicalPath))
                    {
                        System.IO.File.Delete(physicalPath);
                    }
                }
                catch { /* Ignore delete exceptions */ }
            }

            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Agency));
        }
        #endregion
        #region Theme
        [HttpGet]
        public async Task<IActionResult> Themes(int? id)
        {
            var viewModel = new ThemeViewModel
            {
                ThemeList = await _iThemeBL.GetAllThemesAsync()
            };

            if (id.HasValue && id.Value > 0)
            {
                var existing = await _iThemeBL.GetThemeByIdAsync(id.Value);
                if (existing != null)
                {
                    viewModel.FormModel = existing;
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveTheme(ThemeViewModel viewModel)
        {
            // Optional image validation
            if (viewModel.FormModel.ImageFile != null && viewModel.FormModel.ImageFile.Length > 0)
            {
                const long maxFileSize = 2 * 1024 * 1024; // 2 MB
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var extension = Path.GetExtension(viewModel.FormModel.ImageFile.FileName).ToLowerInvariant();

                if (viewModel.FormModel.ImageFile.Length > maxFileSize)
                {
                    ModelState.AddModelError("FormModel.ImageFile", "Image size cannot exceed 2 MB.");
                }

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("FormModel.ImageFile", "Only JPG, JPEG, PNG, and WebP formats are allowed.");
                }
            }
            else
            {
                ModelState.Remove("FormModel.ImageFile");
            }

            if (!ModelState.IsValid)
            {
                viewModel.ThemeList = await _iThemeBL.GetAllThemesAsync();
                return View("Themes", viewModel);
            }

            // File Upload Logic
            if (viewModel.FormModel.ImageFile != null && viewModel.FormModel.ImageFile.Length > 0)
            {
                string uploadFolder = Path.Combine(_env.WebRootPath, "uploads", "ViksitRajasthan");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                // Delete old image if updating
                if (!string.IsNullOrEmpty(viewModel.FormModel.ImageUrl))
                {
                    string oldPhysicalPath = Path.Combine(_env.WebRootPath, viewModel.FormModel.ImageUrl.TrimStart('/', '\\'));
                    if (System.IO.File.Exists(oldPhysicalPath))
                    {
                        try { System.IO.File.Delete(oldPhysicalPath); } catch { }
                    }
                }

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.FormModel.ImageFile.FileName);
                string newFilePath = Path.Combine(uploadFolder, uniqueFileName);

                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    await viewModel.FormModel.ImageFile.CopyToAsync(stream);
                }

                viewModel.FormModel.ImageUrl = "/uploads/ViksitRajasthan/" + uniqueFileName;
            }

            var result = await _iThemeBL.SaveThemeAsync(viewModel.FormModel);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;

            return RedirectToAction(nameof(Themes));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatusTheme(int id)
        {
            var result = await _iThemeBL.ToggleThemeStatusAsync(id);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Themes));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTheme(int id)
        {
            var theme = await _iThemeBL.GetThemeByIdAsync(id);
            var result = await _iThemeBL.DeleteThemeAsync(id);

            if (result.Success && theme != null && !string.IsNullOrEmpty(theme.ImageUrl))
            {
                try
                {
                    string physicalPath = Path.Combine(_env.WebRootPath, theme.ImageUrl.TrimStart('/', '\\'));
                    if (System.IO.File.Exists(physicalPath))
                    {
                        System.IO.File.Delete(physicalPath);
                    }
                }
                catch { }
            }

            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Themes));
        }
        #endregion
        #region subThemes
        [HttpGet]
        public async Task<IActionResult> SubThemes(int? id)
        {
            var viewModel = new SectorViewModel
            {
                SectorList = await _iSubThemesBL.GetAllSectorsAsync(),
                PillarDropdownList = await _iSubThemesBL.GetPillarDropdownAsync()
            };

            if (id.HasValue && id.Value > 0)
            {
                var existing = await _iSubThemesBL.GetSectorByIdAsync(id.Value);
                if (existing != null)
                {
                    viewModel.FormModel = existing;
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveSubThemes(SectorViewModel viewModel)
        {
            

            if (!ModelState.IsValid)
            {
                viewModel.SectorList = await _iSubThemesBL.GetAllSectorsAsync();
                viewModel.PillarDropdownList = await _iSubThemesBL.GetPillarDropdownAsync();
                return View("SubThemes", viewModel);
            }

            var result = await _iSubThemesBL.SaveSectorAsync(viewModel.FormModel);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;

            return RedirectToAction(nameof(SubThemes));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatusSubThemes(int id)
        {
            var result = await _iSubThemesBL.ToggleSectorStatusAsync(id);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(SubThemes));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSubThemes(int id)
        {
            var sector = await _iSubThemesBL.GetSectorByIdAsync(id);
            var result = await _iSubThemesBL.DeleteSectorAsync(id);


            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(SubThemes));
        }
        #endregion
        #region UNSector
        [HttpGet]
        public async Task<IActionResult> Sector(long? id)
        {
            var viewModel = new UNSectorViewModel
            {
                UNSectorList = await _iUNSectorBL.GetAllUNSectorsAsync()
            };

            if (id.HasValue && id.Value > 0)
            {
                var existing = await _iUNSectorBL.GetUNSectorByIdAsync(id.Value);
                if (existing != null)
                {
                    viewModel.FormModel = existing;
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveSector(UNSectorViewModel viewModel)
        {
            // Optional icon file validation
            if (viewModel.FormModel.UNIconFile != null && viewModel.FormModel.UNIconFile.Length > 0)
            {
                const long maxFileSize = 2 * 1024 * 1024; // 2 MB
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".svg" };
                var extension = Path.GetExtension(viewModel.FormModel.UNIconFile.FileName).ToLowerInvariant();

                if (viewModel.FormModel.UNIconFile.Length > maxFileSize)
                {
                    ModelState.AddModelError("FormModel.UNIconFile", "Icon file size cannot exceed 2 MB.");
                }

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("FormModel.UNIconFile", "Only JPG, PNG, WEBP, and SVG icons are allowed.");
                }
            }
            else
            {
                ModelState.Remove("FormModel.UNIconFile");
            }

            if (!ModelState.IsValid)
            {
                viewModel.UNSectorList = await _iUNSectorBL.GetAllUNSectorsAsync();
                return View("Sectors", viewModel);
            }

            // File Upload Handling
            if (viewModel.FormModel.UNIconFile != null && viewModel.FormModel.UNIconFile.Length > 0)
            {
                string uploadFolder = Path.Combine(_env.WebRootPath, "uploads", "UNSectors");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                // Delete old icon if updating
                if (!string.IsNullOrEmpty(viewModel.FormModel.UNIconUrl))
                {
                    string oldPhysicalPath = Path.Combine(_env.WebRootPath, viewModel.FormModel.UNIconUrl.TrimStart('/', '\\'));
                    if (System.IO.File.Exists(oldPhysicalPath))
                    {
                        try { System.IO.File.Delete(oldPhysicalPath); } catch { }
                    }
                }

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.FormModel.UNIconFile.FileName);
                string newFilePath = Path.Combine(uploadFolder, uniqueFileName);

                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    await viewModel.FormModel.UNIconFile.CopyToAsync(stream);
                }

                viewModel.FormModel.UNIconUrl = "/uploads/UNSectors/" + uniqueFileName;
            }

            var result = await _iUNSectorBL.SaveUNSectorAsync(viewModel.FormModel);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;

            return RedirectToAction(nameof(Sector));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatusSector(long id)
        {
            var result = await _iUNSectorBL.ToggleUNSectorStatusAsync(id);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Sector));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSector(long id)
        {
            var entity = await _iUNSectorBL.GetUNSectorByIdAsync(id);
            var result = await _iUNSectorBL.DeleteUNSectorAsync(id);

            if (result.Success && entity != null && !string.IsNullOrEmpty(entity.UNIconUrl))
            {
                try
                {
                    string physicalPath = Path.Combine(_env.WebRootPath, entity.UNIconUrl.TrimStart('/', '\\'));
                    if (System.IO.File.Exists(physicalPath))
                    {
                        System.IO.File.Delete(physicalPath);
                    }
                }
                catch { }
            }

            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Sector));
        }
        #endregion
        #region Group
        [HttpGet]
        public async Task<IActionResult> Group(Guid? guid)
        {
            var viewModel = new GroupViewModel
            {
                GroupList = await _iGroupBL.GetAllGroupsAsync()
            };

            if (guid.HasValue && guid.Value != Guid.Empty)
            {
                var existing = await _iGroupBL.GetGroupByGuidAsync(guid.Value);
                if (existing != null)
                {
                    viewModel.FormModel = existing;
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveGroup(GroupViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.GroupList = await _iGroupBL.GetAllGroupsAsync();
                return View("Group", viewModel);
            }

            var result = await _iGroupBL.SaveGroupAsync(viewModel.FormModel);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;

            return RedirectToAction(nameof(Group));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatusGroup(Guid guid)
        {
            var result = await _iGroupBL.ToggleGroupStatusAsync(guid);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Group));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGroup(Guid guid)
        {
            var result = await _iGroupBL.DeleteGroupAsync(guid);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Group));
        }
        #endregion
        #region Nature Of Support
        [HttpGet]
        public async Task<IActionResult> NatureOfSupport(long? id)
        {
            var viewModel = new NatureOfSupportViewModel
            {
                NatureOfSupportList = await _iNatureOfSupportBL.GetAllAsync()
            };

            if (id.HasValue && id.Value > 0)
            {
                var existing = await _iNatureOfSupportBL.GetByIdAsync(id.Value);
                if (existing != null)
                {
                    viewModel.FormModel = existing;
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveNatureOfSupport(NatureOfSupportViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.NatureOfSupportList = await _iNatureOfSupportBL.GetAllAsync();
                return View("NatureOfSupport", viewModel);
            }

            var result = await _iNatureOfSupportBL.SaveAsync(viewModel.FormModel);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;

            return RedirectToAction(nameof(NatureOfSupport));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatusNatureOfSupport(long id)
        {
            var result = await _iNatureOfSupportBL.ToggleStatusAsync(id);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(NatureOfSupport));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNatureOfSupport(long id)
        {
            var result = await _iNatureOfSupportBL.DeleteAsync(id);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(NatureOfSupport));
        }
        #endregion
        #region SubNatureOfSupport
        [HttpGet]
        public async Task<IActionResult> SubNatureOfSupport(long? id)
        {
            var viewModel = new SubNatureOfSupportViewModel
            {
                SubNatureOfSupportList = await _iSubNatureOfSupportBL.GetAllAsync(),
                NatureOfSupportDropdownList = await _iSubNatureOfSupportBL.GetNatureOfSupportDropdownAsync()
            };

            if (id.HasValue && id.Value > 0)
            {
                var existing = await _iSubNatureOfSupportBL.GetByIdAsync(id.Value);
                if (existing != null)
                {
                    viewModel.FormModel = existing;
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveSubNatureOfSupport(SubNatureOfSupportViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.SubNatureOfSupportList = await _iSubNatureOfSupportBL.GetAllAsync();
                viewModel.NatureOfSupportDropdownList = await _iSubNatureOfSupportBL.GetNatureOfSupportDropdownAsync();
                return View("SubNatureOfSupport", viewModel);
            }

            var result = await _iSubNatureOfSupportBL.SaveAsync(viewModel.FormModel);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;

            return RedirectToAction(nameof(SubNatureOfSupport));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatusSubNatureOfSupport(long id)
        {
            var result = await _iSubNatureOfSupportBL.ToggleStatusAsync(id);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(SubNatureOfSupport));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSubNatureOfSupport(long id)
        {
            var result = await _iSubNatureOfSupportBL.DeleteAsync(id);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(SubNatureOfSupport));
        }
        #endregion
        #region Menu
        [HttpGet]
        public async Task<IActionResult> Menu(Guid? guid)
        {
            var viewModel = new MenuViewModel
            {
                MenuList = await _iMenuBL.GetAllMenusAsync(),
                MainMenuDropdownList = await _iMenuBL.GetParentMenuDropdownAsync()
            };

            if (guid.HasValue && guid.Value != Guid.Empty)
            {
                var existing = await _iMenuBL.GetMenuByGuidAsync(guid.Value);
                if (existing != null)
                {
                    viewModel.FormModel = existing;
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveMenu(MenuViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.MenuList = await _iMenuBL.GetAllMenusAsync();
                viewModel.MainMenuDropdownList = await _iMenuBL.GetParentMenuDropdownAsync();
                return View("Menu", viewModel);
            }

            var result = await _iMenuBL.SaveMenuAsync(viewModel.FormModel);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;

            return RedirectToAction(nameof(Menu));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatusMenu(Guid guid)
        {
            var result = await _iMenuBL.ToggleMenuStatusAsync(guid);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Menu));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMenu(Guid guid)
        {
            var result = await _iMenuBL.DeleteMenuAsync(guid);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Menu));
        }
        #endregion
        #region Contact
        // GET: Contact Listing & Edit Mode
        [HttpGet]
        public async Task<IActionResult> Contact(Guid? guid)
        {
            var viewModel = new ContactViewModel
            {
                ContactList = await _iContactBL.GetAllContactsAsync()
            };

            if (guid.HasValue && guid.Value != Guid.Empty)
            {
                var editData = await _iContactBL.GetContactByGuidAsync(guid.Value);
                if (editData != null)
                {
                    viewModel.FormModel = editData;
                }
            }

            return View(viewModel);
        }

        // POST: Save or Update Contact
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveContact(ContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ContactList = await _iContactBL.GetAllContactsAsync();
                return View("Contact", model);
            }

            try
            {
                // File Upload Handling
                if (model.FormModel.PhotoFile != null && model.FormModel.PhotoFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "contacts");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string fileExtension = Path.GetExtension(model.FormModel.PhotoFile.FileName);
                    string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.FormModel.PhotoFile.CopyToAsync(fileStream);
                    }

                    model.FormModel.PhotoUrl = "/uploads/contacts/" + uniqueFileName;
                }

                bool result = await _iContactBL.SaveContactAsync(model.FormModel);
                if (result)
                {
                    TempData["SuccessMessage"] = model.FormModel.ContactId > 0
                        ? "Contact updated successfully!"
                        : "Contact added successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to save contact. Please try again.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
            }

            return RedirectToAction("Contact");
        }

        // POST: Toggle Active / Inactive Status
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatusContact(Guid guid)
        {
            try
            {
                bool result = await _iContactBL.ToggleStatusAsync(guid);
                if (result)
                {
                    TempData["SuccessMessage"] = "Status updated successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Record not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error changing status: " + ex.Message;
            }

            return RedirectToAction("Contact");
        }

        // POST: Delete Contact
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteContact(Guid guid)
        {
            try
            {
                bool result = await _iContactBL.DeleteContactAsync(guid);
                if (result)
                {
                    TempData["SuccessMessage"] = "Contact deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete contact.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting record: " + ex.Message;
            }

            return RedirectToAction("Contact");
        }
        #endregion
        #region CMDetails
        [HttpGet]
        public async Task<IActionResult> CMDetails(Guid? guid)
        {
            var viewModel = new CMDetailsViewModel
            {
                CMList = await _iCMDetailsBL.GetAllAsync()
            };

            if (guid.HasValue && guid.Value != Guid.Empty)
            {
                var editData = await _iCMDetailsBL.GetByGuidAsync(guid.Value);
                if (editData != null)
                {
                    viewModel.FormModel = editData;
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCMDetails(CMDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.CMList = await _iCMDetailsBL.GetAllAsync();
                return View("CMDetails", model);
            }

            try
            {
                if (model.FormModel.PhotoFile != null && model.FormModel.PhotoFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "cm_details");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string fileExtension = Path.GetExtension(model.FormModel.PhotoFile.FileName);
                    string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.FormModel.PhotoFile.CopyToAsync(fileStream);
                    }

                    model.FormModel.PhotoUrl = "/uploads/cm_details/" + uniqueFileName;
                }

                bool result = await _iCMDetailsBL.SaveAsync(model.FormModel);
                if (result)
                {
                    TempData["SuccessMessage"] = model.FormModel.CMId > 0
                        ? "CM details updated successfully!"
                        : "CM details added successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to save record. Please try again.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
            }

            return RedirectToAction("CMDetails");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatusCMDetails(Guid guid)
        {
            try
            {
                bool result = await _iCMDetailsBL.ToggleStatusAsync(guid);
                if (result)
                {
                    TempData["SuccessMessage"] = "Status updated successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Record not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error changing status: " + ex.Message;
            }

            return RedirectToAction("CMDetails");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCMDetails(Guid guid)
        {
            try
            {
                bool result = await _iCMDetailsBL.DeleteAsync(guid);
                if (result)
                {
                    TempData["SuccessMessage"] = "Record deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete record.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting record: " + ex.Message;
            }

            return RedirectToAction("CMDetails");
        }
        #endregion
        #region NodalDetails
        [HttpGet]
        public async Task<IActionResult> NodalDetail(Guid? guid)
        {
            var viewModel = new NodalDetailViewModel
            {
                NodalList = await _iNodalDetailBL.GetAllAsync()
            };

            if (guid.HasValue && guid.Value != Guid.Empty)
            {
                var editData = await _iNodalDetailBL.GetByGuidAsync(guid.Value);
                if (editData != null)
                {
                    viewModel.FormModel = editData;
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveNodalDetail(NodalDetailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.NodalList = await _iNodalDetailBL.GetAllAsync();
                return View("NodalDetail", model);
            }

            try
            {
                if (model.FormModel.PhotoFile != null && model.FormModel.PhotoFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "nodal_officers");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string fileExtension = Path.GetExtension(model.FormModel.PhotoFile.FileName);
                    string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.FormModel.PhotoFile.CopyToAsync(fileStream);
                    }

                    model.FormModel.PhotoUrl = "/uploads/nodal_officers/" + uniqueFileName;
                }

                bool result = await _iNodalDetailBL.SaveAsync(model.FormModel);
                if (result)
                {
                    TempData["SuccessMessage"] = model.FormModel.NodalId > 0
                        ? "Nodal Officer details updated successfully!"
                        : "Nodal Officer added successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to save record. Please try again.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
            }

            return RedirectToAction("NodalDetail");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatusNodalDetail(Guid guid)
        {
            try
            {
                bool result = await _iNodalDetailBL.ToggleStatusAsync(guid);
                if (result)
                {
                    TempData["SuccessMessage"] = "Status updated successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Record not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error changing status: " + ex.Message;
            }

            return RedirectToAction("NodalDetail");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNodalDetail(Guid guid)
        {
            try
            {
                bool result = await _iNodalDetailBL.DeleteAsync(guid);
                if (result)
                {
                    TempData["SuccessMessage"] = "Nodal Officer record deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete record.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting record: " + ex.Message;
            }

            return RedirectToAction("NodalDetail");
        }
        #endregion
        #region MenuPermission
        // 1. GET: Group change hone par ya page load hone par
        [HttpGet]
        public async Task<IActionResult> MenuPermission(int? groupId)
        {
            ViewBag.Groups = await _iMenuPermissionBL.GetGroupsAsync();

            var model = new MenuPermissionViewModel
            {
                SelectedGroupId = groupId ?? 0
            };

            if (model.SelectedGroupId > 0)
            {
                model.MenuList = await _iMenuPermissionBL.GetGroupMenuPermissionsAsync(model.SelectedGroupId);
            }

            return View(model);
        }

        // 2. POST: Direct Form Submit - No AJAX Needed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SavePermissions(MenuPermissionViewModel model)
        {
            if (model.SelectedGroupId <= 0 || model.MenuList == null)
            {
                TempData["ErrorMessage"] = "Invalid payload or group not selected.";
                return RedirectToAction("MenuPermission", new { groupId = model.SelectedGroupId });
            }

            var saveDto = new SaveGroupPermissionDto
            {
                GroupId = model.SelectedGroupId,
                Permissions = model.MenuList
            };

            string currentUserId = ""; // Claims ya Session se
            await _iMenuPermissionBL.SaveGroupPermissionsAsync(saveDto, currentUserId);

            TempData["SuccessMessage"] = "Permissions saved successfully!";
            return RedirectToAction(nameof(MenuPermission), new { groupId = model.SelectedGroupId });
        }
        #endregion
    }
}
