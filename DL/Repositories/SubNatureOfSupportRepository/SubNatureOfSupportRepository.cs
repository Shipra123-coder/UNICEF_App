using DL.Data;
using Microsoft.EntityFrameworkCore;
using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.SubNatureOfSupportRepository
{
    public class SubNatureOfSupportRepository : ISubNatureOfSupportRepository
    {
        private readonly ApplicationDbContext _context;

        public SubNatureOfSupportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<mst_SubNatureOfSupport>> GetAllAsync()
        {
            return await _context.Set<mst_SubNatureOfSupport>()
                .Include(s => s.NatureOfSupport)
                .AsNoTracking()
                .OrderBy(s => s.NatureOfSupport != null ? s.NatureOfSupport.DisplayOrder : int.MaxValue)
                .ThenBy(s => s.NatureSupportId)
                .ThenBy(s => s.DisplayOrder ?? int.MaxValue)
                .ToListAsync();
        }

        public async Task<mst_SubNatureOfSupport?> GetByIdAsync(long id)
        {
            if (id <= 0) return null;

            return await _context.Set<mst_SubNatureOfSupport>()
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SubNatureOfSupportId == id);
        }

        public async Task<IEnumerable<mst_NatureOfSupport>> GetActiveNatureOfSupportsAsync()
        {
            return await _context.Set<mst_NatureOfSupport>()
                .Where(n => n.IsActive)
                .OrderBy(n => n.DisplayOrder ?? int.MaxValue)
                .ThenBy(n => n.NatureSupportName)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> AddAsync(mst_SubNatureOfSupport model)
        {
            model.CreatedDate = DateTime.Now;

            await _context.Set<mst_SubNatureOfSupport>().AddAsync(model);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(mst_SubNatureOfSupport model)
        {
            var existing = await _context.Set<mst_SubNatureOfSupport>().FindAsync(model.SubNatureOfSupportId);
            if (existing == null) return false;

            existing.NatureSupportId = model.NatureSupportId;
            existing.SupportDetailName = model.SupportDetailName;
            existing.DisplayOrder = model.DisplayOrder;
            existing.IsActive = model.IsActive;

            _context.Set<mst_SubNatureOfSupport>().Update(existing);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.Set<mst_SubNatureOfSupport>().FindAsync(id);
            if (entity == null) return false;

            _context.Set<mst_SubNatureOfSupport>().Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ToggleStatusAsync(long id)
        {
            var entity = await _context.Set<mst_SubNatureOfSupport>().FindAsync(id);
            if (entity == null) return false;

            entity.IsActive = !entity.IsActive;

            _context.Set<mst_SubNatureOfSupport>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
