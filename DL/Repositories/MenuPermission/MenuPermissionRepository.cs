using DL.Data;
using Microsoft.EntityFrameworkCore;
using MO.Entities;
using MO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.MenuPermission
{
    public class MenuPermissionRepository : IMenuPermissionRepository
    {
        private readonly ApplicationDbContext _context;

        public MenuPermissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<m_group>> GetActiveGroupsAsync()
        {
            return await _context.m_group
                .Where(g => g.IsActive == 1)
                .OrderBy(g => g.Name)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<m_Menu>> GetAllActiveMenusAsync()
        {
            return await _context.m_Menu
                .Where(m => m.IsActive == 1)
                .OrderBy(m => m.MainMenuId)
                .ThenBy(m => m.OrderNumber)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<m_MenuPermission>> GetPermissionsByGroupIdAsync(int groupId)
        {
            return await _context.m_MenuPermission
                .Where(p => p.GroupId == groupId && p.IsActive)
                .ToListAsync();
        }

        public async Task SaveGroupPermissionsAsync(int groupId, List<m_MenuPermission> permissions, string modifiedBy)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingList = await _context.m_MenuPermission
                    .Where(p => p.GroupId == groupId)
                    .ToListAsync();

                foreach (var item in permissions)
                {
                    var existing = existingList.FirstOrDefault(p => p.MenuId == item.MenuId);

                    if (existing != null)
                    {
                        existing.CanList = item.CanList;
                        existing.CanAdd = item.CanAdd;
                        existing.CanEdit = item.CanEdit;
                        existing.CanDelete = item.CanDelete;
                        existing.CanActiveDeactive = item.CanActiveDeactive;
                        existing.url = item.url;
                        existing.IsActive = true;
                    }
                    else
                    {
                        item.GroupId = groupId;
                        item.UserId = null;
                        item.IsActive = true;
                        item.CreatedDate = DateTime.Now;
                        item.CreatedBy = modifiedBy;
                        await _context.m_MenuPermission.AddAsync(item);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<m_Menu>> GetAllowedMenusByGroupIdAsync(int? groupId)
        {
            // 1. Agar groupId null ya non-positive hai, toh DB query chalaye bina empty list return karein
            if (!groupId.HasValue || groupId <= 0)
            {
                return new List<m_Menu>();
            }

            int varGroup = groupId.Value;

            // 2. Query with Distinct() to avoid duplicate menus
            var query = from menu in _context.m_Menu.AsNoTracking()
                        join perm in _context.m_MenuPermission.AsNoTracking()
                            on menu.MenuId equals perm.MenuId
                        where perm.GroupId == varGroup
                              && perm.IsActive == true
                              && perm.CanList == true
                              && menu.IsActive == 1
                        orderby menu.OrderNumber
                        select menu;

            return await query.Distinct().ToListAsync();
        }
    }
}
