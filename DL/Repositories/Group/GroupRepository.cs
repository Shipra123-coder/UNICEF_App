using DL.Data;
using Microsoft.EntityFrameworkCore;
using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.Group
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<m_group>> GetAllAsync()
        {
            return await _context.Set<m_group>()
                .AsNoTracking()
                .OrderBy(g => g.Id)
                .ToListAsync();
        }

        public async Task<m_group?> GetByGuidAsync(Guid guid)
        {
            if (guid == Guid.Empty) return null;

            return await _context.Set<m_group>()
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Guid == guid);
        }

        public async Task<bool> AddAsync(m_group model)
        {
            model.Guid = Guid.NewGuid();
            model.CreatedDate = DateTime.Now;
            model.IsActive ??= 1;

            await _context.Set<m_group>().AddAsync(model);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(m_group model)
        {
            var existing = await _context.Set<m_group>().FirstOrDefaultAsync(g => g.Guid == model.Guid);
            if (existing == null) return false;

            existing.Name = model.Name;
            existing.Description = model.Description;
            existing.IsActive = model.IsActive;
            existing.UpdatedBy = model.UpdatedBy;
            existing.UpdatedDate = DateTime.Now;

            _context.Set<m_group>().Update(existing);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid guid)
        {
            var entity = await _context.Set<m_group>().FirstOrDefaultAsync(g => g.Guid == guid);
            if (entity == null) return false;

            _context.Set<m_group>().Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ToggleStatusAsync(Guid guid)
        {
            var entity = await _context.Set<m_group>().FirstOrDefaultAsync(g => g.Guid == guid);
            if (entity == null) return false;

            entity.IsActive = (entity.IsActive == 1) ? 0 : 1;
            entity.UpdatedDate = DateTime.Now;

            _context.Set<m_group>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
