using MO.Entities;
using MO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.MenuPermission
{
    public interface IMenuPermissionRepository
    {
        Task<List<m_group>> GetActiveGroupsAsync();
        Task<List<m_Menu>> GetAllActiveMenusAsync();
        Task<List<m_MenuPermission>> GetPermissionsByGroupIdAsync(int groupId);
        Task SaveGroupPermissionsAsync(int groupId, List<m_MenuPermission> permissions, string modifiedBy);
        Task<List<m_Menu>> GetAllowedMenusByGroupIdAsync(int? groupId);
    }
}
