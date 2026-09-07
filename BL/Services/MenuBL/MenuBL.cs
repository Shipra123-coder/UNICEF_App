using DL.Repositories.MenuRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.MenuBL
{
    public class MenuBL : IMenuBL
    {
        private readonly IMenuRepository _menuRepo;

        public MenuBL(IMenuRepository menuRepo)
        {
            _menuRepo = menuRepo;
        }

        public async Task<IEnumerable<m_Menu>> GetAllMenusAsync()
        {
            return await _menuRepo.GetAllAsync();
        }

        public async Task<m_Menu?> GetMenuByGuidAsync(Guid guid)
        {
            if (guid == Guid.Empty) return null;
            return await _menuRepo.GetByGuidAsync(guid);
        }

        public async Task<IEnumerable<SelectListItem>> GetParentMenuDropdownAsync()
        {
            var parentMenus = await _menuRepo.GetParentMenusAsync();
            return parentMenus.Select(p => new SelectListItem
            {
                Value = p.MenuId.ToString(),
                Text = string.IsNullOrEmpty(p.MangalName) ? p.Name : $"{p.Name} ({p.MangalName})"
            }).ToList();
        }

        public async Task<(bool Success, string Message)> SaveMenuAsync(m_Menu model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                return (false, "Menu Name is required.");
            }

            if (model.IsSubMenu == true && (!model.MainMenuId.HasValue || model.MainMenuId <= 0))
            {
                return (false, "Please select a Parent Menu for this SubMenu.");
            }

            if (!model.Guid.HasValue || model.Guid == Guid.Empty)
            {
                bool isAdded = await _menuRepo.AddAsync(model);
                return isAdded ? (true, "Menu created successfully.") : (false, "Failed to create menu.");
            }
            else
            {
                bool isUpdated = await _menuRepo.UpdateAsync(model);
                return isUpdated ? (true, "Menu updated successfully.") : (false, "Failed to update menu.");
            }
        }

        public async Task<(bool Success, string Message)> DeleteMenuAsync(Guid guid)
        {
            if (guid == Guid.Empty) return (false, "Invalid Menu identifier.");
            bool isDeleted = await _menuRepo.DeleteAsync(guid);
            return isDeleted ? (true, "Menu deleted successfully.") : (false, "Failed to delete menu.");
        }

        public async Task<(bool Success, string Message)> ToggleMenuStatusAsync(Guid guid)
        {
            if (guid == Guid.Empty) return (false, "Invalid Menu identifier.");
            bool isToggled = await _menuRepo.ToggleStatusAsync(guid);
            return isToggled ? (true, "Status updated successfully.") : (false, "Failed to update status.");
        }
    }
}
