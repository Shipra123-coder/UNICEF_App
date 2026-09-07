using MO.Entities;
using MO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.MenuPermission
{
    public interface IMenuPermissionBL
    {
        Task<List<m_group>> GetGroupsAsync();
        Task<List<GroupMenuPermission>> GetGroupMenuPermissionsAsync(int groupId);
        Task<bool> SaveGroupPermissionsAsync(SaveGroupPermissionDto dto, string currentUserId);
        Task<List<m_Menu>> GetAllowedMenusAsync(int? groupId);

    }
}
