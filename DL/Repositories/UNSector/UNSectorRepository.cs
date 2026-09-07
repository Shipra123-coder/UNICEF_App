using DL.Data;
using Microsoft.EntityFrameworkCore;
using MO.Entities;


namespace DL.Repositories.UNSector
{
    public class UNSectorRepository : IUNSectorRepository
    {
        private readonly ApplicationDbContext _context;

        public UNSectorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<mst_UNSector>> GetAllAsync()
        {
            return await _context.Set<mst_UNSector>().AsNoTracking()
                .OrderBy(s => s.UNSectorId)
                .ToListAsync();
        }

        public async Task<mst_UNSector?> GetByIdAsync(long id)
        {
            if (id <= 0) return null;

            return await _context.Set<mst_UNSector>()
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.UNSectorId == id);
        }

        public async Task<bool> AddAsync(mst_UNSector model)
        {
            model.CreatedDate = DateTime.Now;
            model.IsActive ??= 1;

            await _context.Set<mst_UNSector>().AddAsync(model);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(mst_UNSector model)
        {
            var existing = await _context.Set<mst_UNSector>().FindAsync(model.UNSectorId);
            if (existing == null) return false;

            existing.UNSectorName = model.UNSectorName;
            existing.UNSectorCode = model.UNSectorCode;
            existing.UNDescription = model.UNDescription;
            existing.ColorCode = model.ColorCode;
            existing.IsActive = model.IsActive;
            existing.UNIconUrl = model.UNIconUrl;

            _context.Set<mst_UNSector>().Update(existing);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.Set<mst_UNSector>().FindAsync(id);
            if (entity == null) return false;

            _context.Set<mst_UNSector>().Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ToggleStatusAsync(long id)
        {
            var entity = await _context.Set<mst_UNSector>().FindAsync(id);
            if (entity == null) return false;

            entity.IsActive = (entity.IsActive == 1) ? 0 : 1;

            _context.Set<mst_UNSector>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}