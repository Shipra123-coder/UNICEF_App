using DL.Repositories.MenuPermission;
using Microsoft.EntityFrameworkCore;
using MO.Entities;
using MO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.MenuPermission
{
    public class MenuPermissionBL : IMenuPermissionBL
    {
        private readonly IMenuPermissionRepository _repo;

        public MenuPermissionBL(IMenuPermissionRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<m_group>> GetGroupsAsync()
        {
            return await _repo.GetActiveGroupsAsync();
        }

        public async Task<List<GroupMenuPermission>> GetGroupMenuPermissionsAsync(int groupId)
        {
            var menus = await _repo.GetAllActiveMenusAsync();
            var permissions = await _repo.GetPermissionsByGroupIdAsync(groupId);

            var list = new List<GroupMenuPermission>();

            foreach (var m in menus)
            {
                var perm = permissions.FirstOrDefault(p => p.MenuId == m.MenuId);

                list.Add(new GroupMenuPermission
                {
                    MenuId = m.MenuId,
                    Name = m.Name,
                    MangalName = m.MangalName,
                    Controller = m.Controller,
                    ActionName = m.ActionName,
                    Icon = m.Icon,
                    IsSubMenu = m.IsSubMenu,
                    MainMenuId = m.MainMenuId,
                    OrderNumber = m.OrderNumber,
                    PermissionId = perm?.PermissionId ?? 0,
                    CanList = perm?.CanList ?? false,
                    CanAdd = perm?.CanAdd ?? false,
                    CanEdit = perm?.CanEdit ?? false,
                    CanDelete = perm?.CanDelete ?? false,
                    CanActiveDeactive = perm?.CanActiveDeactive ?? false
                });
            }

            return list;
        }

        public async Task<bool> SaveGroupPermissionsAsync(SaveGroupPermissionDto dto, string currentUserId)
        {
            if (dto.GroupId <= 0)
                throw new ArgumentException("Please select a valid Group.");

            var permissions = dto.Permissions.Select(p => new m_MenuPermission
            {
                MenuId = p.MenuId,
                GroupId = dto.GroupId,
                url = (!string.IsNullOrEmpty(p.Controller) && p.Controller != "#")
                        ? $"/{p.Controller}/{p.ActionName}"
                        : "#",
                CanList = p.CanList,
                CanAdd = p.CanAdd,
                CanEdit = p.CanEdit,
                CanDelete = p.CanDelete,
                CanActiveDeactive = p.CanActiveDeactive
            }).ToList();

            await _repo.SaveGroupPermissionsAsync(dto.GroupId, permissions, currentUserId);
            return true;
        }

        public async Task<List<m_Menu>> GetAllowedMenusAsync(int? groupId)
        {
            return await _repo.GetAllowedMenusByGroupIdAsync(groupId);
        }
    }
}
