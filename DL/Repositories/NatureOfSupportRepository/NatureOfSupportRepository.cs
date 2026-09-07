using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.Data;
using Microsoft.EntityFrameworkCore;
using MO.Entities;

namespace MO.Repositories
{
    public class NatureOfSupportRepository : INatureOfSupportRepository
    {
        private readonly ApplicationDbContext _context;

        public NatureOfSupportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<mst_NatureOfSupport>> GetAllAsync()
        {
            return await _context.Set<mst_NatureOfSupport>()
                .AsNoTracking()
                .OrderBy(n => n.DisplayOrder ?? int.MaxValue)
                .ThenBy(n => n.NatureSupportId)
                .ToListAsync();
        }

        public async Task<mst_NatureOfSupport?> GetByIdAsync(long id)
        {
            if (id <= 0) return null;

            return await _context.Set<mst_NatureOfSupport>()
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.NatureSupportId == id);
        }

        public async Task<bool> AddAsync(mst_NatureOfSupport model)
        {
            model.CreatedDate = DateTime.Now;

            await _context.Set<mst_NatureOfSupport>().AddAsync(model);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(mst_NatureOfSupport model)
        {
            var existing = await _context.Set<mst_NatureOfSupport>().FindAsync(model.NatureSupportId);
            if (existing == null) return false;

            existing.NatureSupportName = model.NatureSupportName;
            existing.DisplayOrder = model.DisplayOrder;
            existing.IsActive = model.IsActive;

            _context.Set<mst_NatureOfSupport>().Update(existing);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.Set<mst_NatureOfSupport>().FindAsync(id);
            if (entity == null) return false;

            _context.Set<mst_NatureOfSupport>().Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ToggleStatusAsync(long id)
        {
            var entity = await _context.Set<mst_NatureOfSupport>().FindAsync(id);
            if (entity == null) return false;

            entity.IsActive = !entity.IsActive;

            _context.Set<mst_NatureOfSupport>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}