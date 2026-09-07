using BL.Services.Permission;
using BL.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MO.Common;
using MO.ViewModels;
using System.Net;
using UNICEF_App.Models;

namespace UNICEF_App.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserBL _userBL;
        private readonly IPermissionBL _iPermissionBL;
        public UserController(IUserBL userBL,IPermissionBL iPermissionBL)
        {
            _userBL = userBL;
            _iPermissionBL = iPermissionBL;
        }

        [HttpGet]
        [HasPermission(PermissionAction.Add)]
        public async Task<IActionResult> Create()
        {
            var model = await _userBL.GetUserFormDataAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(PermissionAction.Add)]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string currentUser = User.Identity?.Name ?? "Admin";
                var (success, message) = await _userBL.SaveUserAsync(model, currentUser);

                if (success)
                {
                    TempData["SuccessMessage"] = message;
                    return RedirectToAction(nameof(Create));
                }

                ModelState.AddModelError(string.Empty, message);
            }

            // Reload dropdown data on failure/validation error
            var dropdowns = await _userBL.GetUserFormDataAsync();
            model.GroupList = dropdowns.GroupList;
            model.UserLevelList = dropdowns.UserLevelList;
            model.AgencyList = dropdowns.AgencyList;
            model.DepartmentList = dropdowns.DepartmentList;
            return View(model);
        }

        [HttpGet]
        [HasPermission(PermissionAction.List)]
        public async Task<IActionResult> List()
        {
            var userList = await _userBL.GetAllUsersAsync();
            return View(userList);
        }


        // GET: /User/Edit/5
        [HttpGet]
        [HasPermission(PermissionAction.Edit)]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _userBL.GetUserForEditAsync(id);
            if (model == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction(nameof(Index));
            }

            // Dropdowns load karne ke liye helper method (Agency, Dept, Roles etc.)
            //await LoadDropdownsAsync(model.GroupId, model.UserLevel);
            return View(model);
        }

        // POST: /User/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(PermissionAction.Edit)]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            // 1. Model Validation Check
            if (!ModelState.IsValid)
            {
                // Dropdowns reload karna zaroori hai taaki form crash na ho
                var drop = await _userBL.GetUserFormDataAsync();
                model.GroupList = drop.GroupList;
                model.UserLevelList = drop.UserLevelList;
                model.DepartmentList = drop.DepartmentList;
                model.AgencyList = drop.AgencyList;
                return View(model);
            }

            try
            {
                // 2. Logged-in username safely fetch karein
                string updatedBy = !string.IsNullOrWhiteSpace(User.Identity?.Name)
                    ? User.Identity.Name
                    : "Admin";

                // 3. BL Update Call
                var (success, message) = await _userBL.UpdateUserAsync(model, updatedBy);

                if (!success)
                {
                    ModelState.AddModelError(string.Empty, message ?? "User update karne mein samasya aayi.");
                    var drop = await _userBL.GetUserFormDataAsync();
                    model.GroupList = drop.GroupList;
                    model.UserLevelList = drop.UserLevelList;
                    model.DepartmentList = drop.DepartmentList;
                    model.AgencyList = drop.AgencyList;
                    return View(model);
                }

                // 4. Success Case
                TempData["SuccessMessage"] = message ?? "User successfully update ho gaya.";
                return RedirectToAction(nameof(List));
            }
            catch (Exception ex)
            {
                // Log ex yahan add kar sakte hain: _logger.LogError(ex, "Error updating user");
                ModelState.AddModelError(string.Empty, "Ek unexpected error aayi. Kripya dobara prayas karein.");
                var drop = await _userBL.GetUserFormDataAsync();
                model.GroupList = drop.GroupList;
                model.UserLevelList = drop.UserLevelList;
                model.DepartmentList = drop.DepartmentList;
                model.AgencyList = drop.AgencyList;
                return View(model);
            }
        }
    }
}
