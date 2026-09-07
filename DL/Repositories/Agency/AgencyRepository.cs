using DL.Data;
using Microsoft.EntityFrameworkCore;
using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.Agency
{
    public class AgencyRepository : IAgencyRepository
    {
        private readonly ApplicationDbContext _context;

        public AgencyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<mst_Agency>> GetAllAsync()
        {
            return await _context.Set<mst_Agency>().AsNoTracking()
                .OrderByDescending(a => a.AgencyId)
                .ToListAsync();
        }

        public async Task<mst_Agency?> GetByGuidAsync(Guid guid)
        {
            if (guid == Guid.Empty) return null;

            return await _context.Set<mst_Agency>()
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Guid == guid);
        }

        public async Task<bool> AddAsync(mst_Agency model)
        {
            model.Guid = Guid.NewGuid();
            model.CreatedDate = DateTime.Now;
            model.Status ??= 1;

            await _context.Set<mst_Agency>().AddAsync(model);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(mst_Agency model)
        {
            var existing = await _context.Set<mst_Agency>().FirstOrDefaultAsync(a => a.Guid == model.Guid);
            if (existing == null) return false;

            existing.AgencyName = model.AgencyName;
            existing.AgencyCode = model.AgencyCode;
            existing.Description = model.Description;
            existing.Websitelink = model.Websitelink;
            existing.Status = model.Status;
            existing.LogoURL = model.LogoURL;
            existing.UpdatedBy = model.UpdatedBy;
            existing.UpdatedDate = DateTime.Now;

            _context.Set<mst_Agency>().Update(existing);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid guid)
        {
            var entity = await _context.Set<mst_Agency>().FirstOrDefaultAsync(a => a.Guid == guid);
            if (entity == null) return false;

            _context.Set<mst_Agency>().Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ToggleStatusAsync(Guid guid)
        {
            var entity = await _context.Set<mst_Agency>().FirstOrDefaultAsync(a => a.Guid == guid);
            if (entity == null) return false;

            entity.Status = (entity.Status == 1) ? 0 : 1;
            entity.UpdatedDate = DateTime.Now;

            _context.Set<mst_Agency>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
