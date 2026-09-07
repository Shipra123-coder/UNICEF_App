using DL.Data;
using Microsoft.EntityFrameworkCore;
using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.MenuRepository
{
    public class MenuRepository : IMenuRepository
    {
        private readonly ApplicationDbContext _context;

        public MenuRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<m_Menu>> GetAllAsync()
        {
            return await _context.Set<m_Menu>()
                .Include(m => m.ParentMenu)
                .AsNoTracking()
                .OrderBy(m => m.MainMenuId.HasValue ? m.MainMenuId : m.MenuId)
                .ThenBy(m => m.OrderNumber ?? int.MaxValue)
                .ToListAsync();
        }

        public async Task<m_Menu?> GetByGuidAsync(Guid guid)
        {
            if (guid == Guid.Empty) return null;

            return await _context.Set<m_Menu>()
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Guid == guid);
        }

        public async Task<IEnumerable<m_Menu>> GetParentMenusAsync()
        {
            return await _context.Set<m_Menu>()
                .Where(m => (m.IsSubMenu == false || m.MainMenuId == null) && m.IsActive == 1)
                .OrderBy(m => m.OrderNumber ?? int.MaxValue)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> AddAsync(m_Menu model)
        {
            model.Guid = Guid.NewGuid();
            model.CreatedDate = DateTime.Now;
            model.IsActive ??= 1;

            if (model.IsSubMenu == false)
            {
                model.MainMenuId = null;
            }

            await _context.Set<m_Menu>().AddAsync(model);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(m_Menu model)
        {
            var existing = await _context.Set<m_Menu>().FirstOrDefaultAsync(m => m.Guid == model.Guid);
            if (existing == null) return false;

            existing.Name = model.Name;
            existing.MangalName = model.MangalName;
            existing.Controller = model.Controller;
            existing.ActionName = model.ActionName;
            existing.IsSubMenu = model.IsSubMenu;
            existing.MainMenuId = model.IsSubMenu == true ? model.MainMenuId : null;
            existing.OrderNumber = model.OrderNumber;
            existing.Icon = model.Icon;
            existing.IsActive = model.IsActive;
            existing.UpdatedDate = DateTime.Now;
            existing.UpdatedBy = model.UpdatedBy;

            _context.Set<m_Menu>().Update(existing);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid guid)
        {
            var entity = await _context.Set<m_Menu>().FirstOrDefaultAsync(m => m.Guid == guid);
            if (entity == null) return false;

            _context.Set<m_Menu>().Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ToggleStatusAsync(Guid guid)
        {
            var entity = await _context.Set<m_Menu>().FirstOrDefaultAsync(m => m.Guid == guid);
            if (entity == null) return false;

            entity.IsActive = (entity.IsActive == 1) ? 0 : 1;
            entity.UpdatedDate = DateTime.Now;

            _context.Set<m_Menu>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
