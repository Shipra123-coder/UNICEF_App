using DL.Data;
using Microsoft.EntityFrameworkCore;
using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.SubThemes
{
    public class SubThemesRepository : ISubThemesRepository
    {
        private readonly ApplicationDbContext _context;

        public SubThemesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<mst_Sector>> GetAllAsync()
        {
            return await _context.Set<mst_Sector>()
                .Include(s => s.Pillar)
                .AsNoTracking()
                .OrderBy(s => s.PillarId)
                .ThenBy(s => s.SectorId)
                .ToListAsync();
        }

        public async Task<mst_Sector?> GetByIdAsync(int sectorId)
        {
            if (sectorId <= 0) return null;

            return await _context.Set<mst_Sector>()
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SectorId == sectorId);
        }

        public async Task<IEnumerable<mst_Pillar>> GetActivePillarsAsync()
        {
            return await _context.Set<mst_Pillar>()
                .Where(p => p.Status == 1)
                .OrderBy(p => p.PillarId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> AddAsync(mst_Sector model)
        {
            model.CreatedDate = DateTime.Now;
            model.Status ??= 1;

            await _context.Set<mst_Sector>().AddAsync(model);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(mst_Sector model)
        {
            var existing = await _context.Set<mst_Sector>().FindAsync(model.SectorId);
            if (existing == null) return false;

            existing.PillarId = model.PillarId;
            existing.SectorCode = model.SectorCode;
            existing.SectorName = model.SectorName;
            existing.Description = model.Description;
            existing.Status = model.Status;            
            existing.UpdatedBy = model.UpdatedBy;
            existing.UpdatedDate = DateTime.Now;

            _context.Set<mst_Sector>().Update(existing);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int sectorId)
        {
            var entity = await _context.Set<mst_Sector>().FindAsync(sectorId);
            if (entity == null) return false;

            _context.Set<mst_Sector>().Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ToggleStatusAsync(int sectorId)
        {
            var entity = await _context.Set<mst_Sector>().FindAsync(sectorId);
            if (entity == null) return false;

            entity.Status = (entity.Status == 1) ? 0 : 1;
            entity.UpdatedDate = DateTime.Now;

            _context.Set<mst_Sector>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
