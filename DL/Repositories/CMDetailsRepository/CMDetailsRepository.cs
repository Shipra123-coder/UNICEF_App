using DL.Data;
using Microsoft.EntityFrameworkCore;
using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.CMDetailsRepository
{
    public class CMDetailsRepository : ICMDetailsRepository
    {
        private readonly ApplicationDbContext _context;

        public CMDetailsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<mst_CMDetails>> GetAllAsync()
        {
            return await _context.Set<mst_CMDetails>()
                .AsNoTracking()
                .OrderBy(c => c.DisplayOrder)
                .ThenByDescending(c => c.CMId)
                .ToListAsync();
        }

        public async Task<mst_CMDetails> GetByIdAsync(Guid guid)
        {
            if (guid == Guid.Empty) return null;

            return await _context.Set<mst_CMDetails>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Guid == guid);
        }

        public async Task<bool> AddAsync(mst_CMDetails model)
        {
            model.Guid = Guid.NewGuid();
            model.CreatedDate = DateTime.Now;
            model.IsActive = 1;

            await _context.Set<mst_CMDetails>().AddAsync(model);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(mst_CMDetails model)
        {
            var existing = await _context.Set<mst_CMDetails>().FindAsync(model.CMId);
            if (existing == null) return false;

            existing.Name = model.Name;
            existing.Designation = model.Designation;
            existing.PhotoUrl = model.PhotoUrl;
            existing.DisplayOrder = model.DisplayOrder;
            existing.IsActive = model.IsActive;
            existing.UpdatedBy = model.UpdatedBy;
            existing.UpdatedDate = DateTime.Now;

            _context.Set<mst_CMDetails>().Update(existing);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid guid)
        {
            if (guid == Guid.Empty) return false;

            var entity = await _context.Set<mst_CMDetails>()
                .FirstOrDefaultAsync(x => x.Guid == guid);

            if (entity == null) return false;

            _context.Set<mst_CMDetails>().Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ToggleStatusAsync(Guid guid)
        {
            if (guid == Guid.Empty) return false;

            var entity = await _context.Set<mst_CMDetails>()
                .FirstOrDefaultAsync(x => x.Guid == guid);

            if (entity == null) return false;

            entity.IsActive = (entity.IsActive == 1) ? 0 : 1;
            entity.UpdatedDate = DateTime.Now;

            _context.Set<mst_CMDetails>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
