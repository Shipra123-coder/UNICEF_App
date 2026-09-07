using Microsoft.AspNetCore.Mvc.Rendering;
using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.MenuBL
{
    public interface IMenuBL
    {
        Task<IEnumerable<m_Menu>> GetAllMenusAsync();
        Task<m_Menu?> GetMenuByGuidAsync(Guid guid);
        Task<IEnumerable<SelectListItem>> GetParentMenuDropdownAsync();
        Task<(bool Success, string Message)> SaveMenuAsync(m_Menu model);
        Task<(bool Success, string Message)> DeleteMenuAsync(Guid guid);
        Task<(bool Success, string Message)> ToggleMenuStatusAsync(Guid guid);
    }
}
