using DL.Data;
using Microsoft.EntityFrameworkCore;
using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.ThemeRepository
{
    public class ThemeRepository : IThemeRepository
    {
        private readonly ApplicationDbContext _context;

        public ThemeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<mst_Pillar>> GetAllAsync()
        {
            return await _context.Set<mst_Pillar>().AsNoTracking()
                .OrderBy(t => t.PillarId)
                .ToListAsync();
        }

        public async Task<mst_Pillar?> GetByIdAsync(int id)
        {
            if (id <= 0) return null;

            return await _context.Set<mst_Pillar>()
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.PillarId == id);
        }

        public async Task<bool> AddAsync(mst_Pillar model)
        {
            model.CreatedDate = DateTime.Now;
            model.Status ??= 1;

            await _context.Set<mst_Pillar>().AddAsync(model);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(mst_Pillar model)
        {
            var existing = await _context.Set<mst_Pillar>().FindAsync(model.PillarId);
            if (existing == null) return false;

            existing.PillarName = model.PillarName;
            existing.Discription = model.Discription;
            existing.Status = model.Status;
            existing.ImageUrl = model.ImageUrl;
            existing.UpdatedBy = model.UpdatedBy;
            existing.UpdatedDate = DateTime.Now;

            _context.Set<mst_Pillar>().Update(existing);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Set<mst_Pillar>().FindAsync(id);
            if (entity == null) return false;

            _context.Set<mst_Pillar>().Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ToggleStatusAsync(int id)
        {
            var entity = await _context.Set<mst_Pillar>().FindAsync(id);
            if (entity == null) return false;

            entity.Status = (entity.Status == 1) ? 0 : 1;
            entity.UpdatedDate = DateTime.Now;

            _context.Set<mst_Pillar>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
