using DL.Repositories.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using MO.Entities;
using MO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.User
{

    public class UserBL : IUserBL
    {
        private readonly IUserRepository _userRepo;
        private readonly PasswordHasher<Login_User> _passwordHasher;

        public UserBL(IUserRepository userRepo)
        {
            _userRepo = userRepo;
            _passwordHasher = new PasswordHasher<Login_User>();
        }

        public async Task<UserCreateViewModel> GetUserFormDataAsync()
        {
            var groups = await _userRepo.GetActiveGroupsAsync();
            var levels = await _userRepo.GetUserLevelsAsync();
            var agencies = await _userRepo.GetActiveAgenciesAsync();
            var department = await _userRepo.GetActiveDepartmentAsync();

            return new UserCreateViewModel
            {
                GroupList = groups.Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Name }),
                UserLevelList = levels.Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Name }),
                AgencyList = agencies.Select(a => new SelectListItem { Value = a.AgencyId.ToString(), Text = a.AgencyName }),
                DepartmentList = department.Select(a => new SelectListItem { Value = a.DepartmentId.ToString(), Text = a.DepartmentName })
            };
        }

        public async Task<(bool Success, string Message)> SaveUserAsync(UserCreateViewModel model, string createdBy)
        {
            if (await _userRepo.IsSSOExistsAsync(model.SSOID))
            {
                return (false, "SSOID already exists. Please choose a different one.");
            }

            var entity = new Login_User
            {
                Guid = Guid.NewGuid(),
                Name = model.Name.Trim(),
                Password = model.Password,
                DisplayName = model.DisplayName.Trim(),
                SSOID = model.SSOID.Trim(),
                GroupId = model.GroupId,
                UserLevel = model.UserLevel,
                AgencyId = (model.GroupId == 3 || model.UserLevel == 2) ? model.AgencyId : null,
                DepartmentId = (model.GroupId == 2 || model.UserLevel == 1) ? model.DepartmentId : null,
                DistrictId = model.DistrictId,
                RoleId = model.RoleId,
                Designation = model.Designation,
                Mobile = model.Mobile,
                WhatsappMobile = model.WhatsappMobile,
                EmailId = model.EmailId,
                IsActive = model.IsActive ? 1 : 0,
                CreatedDate = DateTime.Now,
                CreatedBy = createdBy                
            };

            // Secure Hashing: Plain password save nahi hoga
            //entity.Password = _passwordHasher.HashPassword(entity, model.Password);

            bool isSaved = await _userRepo.AddUserAsync(entity);
            return isSaved ? (true, "User created successfully!") : (false, "Error occurred while saving the user.");
        }

        public async Task<IEnumerable<UserListViewModel>> GetAllUsersAsync()
        {
            var users = await _userRepo.GetAllUsersAsync();

            return users.Select(u => new UserListViewModel
            {
                Id = u.UserId,
                Guid = u.Guid,
                Name = u.Name,
                DisplayName = u.DisplayName,
                SSOID = u.SSOID,
                EmailId = u.EmailId,
                Mobile = u.Mobile,
                Designation = u.Designation,
                GroupId = u.GroupId,
                UserLevel = u.UserLevel,
                DepartmentId = u.DepartmentId,
                AgencyId = u.AgencyId,
                DistrictId = u.DistrictId,
                RoleId = u.RoleId,
                IsActive = u.IsActive == 1,               
            }).ToList();
        }
    
        public async Task<UserEditViewModel?> GetUserForEditAsync(long id)
        {
            var groups = await _userRepo.GetActiveGroupsAsync();
            var levels = await _userRepo.GetUserLevelsAsync();
            var agencies = await _userRepo.GetActiveAgenciesAsync();
            var department = await _userRepo.GetActiveDepartmentAsync();

            var entity = await _userRepo.GetUserByIdAsync(id);
            if (entity == null) return null;

            return new UserEditViewModel
            {
                Id = entity.UserId,
                Name = entity.Name,
                DisplayName = entity.DisplayName,
                SSOID = entity.SSOID,
                GroupId = entity.GroupId,
                UserLevel = entity.UserLevel,
                AgencyId = entity.AgencyId,
                DepartmentId = entity.DepartmentId,
                DistrictId = entity.DistrictId,
                RoleId = entity.RoleId,
                Designation = entity.Designation,
                Mobile = entity.Mobile,
                WhatsappMobile = entity.WhatsappMobile,
                EmailId = entity.EmailId,
                IsActive = entity.IsActive == 1,

                GroupList = groups.Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Name }),
                UserLevelList = levels.Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Name }),
                AgencyList = agencies.Select(a => new SelectListItem { Value = a.AgencyId.ToString(), Text = a.AgencyName }),
                DepartmentList = department.Select(a => new SelectListItem { Value = a.DepartmentId.ToString(), Text = a.DepartmentName })

            };
        }

        // Edit submit karne ke liye (SaveUserAsync ke matching rules ke sath)
        public async Task<(bool Success, string Message)> UpdateUserAsync(UserEditViewModel model, string updatedBy)
        {
            if (await _userRepo.IsSSOExistsForOtherUserAsync(model.SSOID, model.Id))
            {
                return (false, "SSOID already exists for another user. Please choose a different one.");
            }

            var entity = await _userRepo.GetUserByIdAsync(model.Id);
            if (entity == null)
            {
                return (false, "User not found.");
            }

            // Update mapped properties
            entity.Name = model.Name.Trim();
            entity.DisplayName = model.DisplayName.Trim();
            entity.SSOID = model.SSOID.Trim();
            entity.GroupId = model.GroupId ?? 0;
            entity.UserLevel = model.UserLevel;

            // SaveUserAsync ke business logic ke anusar
            entity.AgencyId = (model.GroupId == 3 || model.UserLevel == 2) ? model.AgencyId : null;
            entity.DepartmentId = (model.GroupId == 2 || model.UserLevel == 1) ? model.DepartmentId : null;

            entity.DistrictId = model.DistrictId;
            entity.RoleId = model.RoleId;
            entity.Designation = model.Designation;
            entity.Mobile = model.Mobile;
            entity.WhatsappMobile = model.WhatsappMobile;
            entity.EmailId = model.EmailId;
            entity.IsActive = model.IsActive ? 1 : 0;

            // Audit fields
            entity.UpdatedDate = DateTime.Now;
            entity.UpdatedBy = updatedBy;

            // Optional: Password update logic agar user ne naya password daala ho
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                entity.Password = model.Password;
                // entity.Password = _passwordHasher.HashPassword(entity, model.Password);
            }

            bool isUpdated = await _userRepo.UpdateUserAsync(entity);
            return isUpdated ? (true, "User updated successfully!") : (false, "Error occurred while updating the user.");
        }

    }
}
