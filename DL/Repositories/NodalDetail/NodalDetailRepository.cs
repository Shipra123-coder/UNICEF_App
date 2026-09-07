using DL.Data;
using Microsoft.EntityFrameworkCore;
using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.NodalDetail
{
    public class NodalDetailRepository : INodalDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public NodalDetailRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<mst_NodalDetail>> GetAllAsync()
        {
            return await _context.Set<mst_NodalDetail>()
                .AsNoTracking()
                .OrderBy(c => c.DisplayOrder)
                .ThenByDescending(c => c.NodalId)
                .ToListAsync();
        }

        public async Task<mst_NodalDetail> GetByIdAsync(Guid guid)
        {
            if (guid == Guid.Empty) return null;

            return await _context.Set<mst_NodalDetail>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Guid == guid);
        }

        public async Task<bool> AddAsync(mst_NodalDetail model)
        {
            model.Guid = Guid.NewGuid();
            model.CreatedDate = DateTime.Now;
            model.IsActive = 1;

            await _context.Set<mst_NodalDetail>().AddAsync(model);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(mst_NodalDetail model)
        {
            var existing = await _context.Set<mst_NodalDetail>().FindAsync(model.NodalId);
            if (existing == null) return false;

            existing.OfficerName = model.OfficerName;
            existing.Designation = model.Designation;
            existing.ContactNo = model.ContactNo;
            existing.EmailId = model.EmailId;
            existing.PhotoUrl = model.PhotoUrl;
            existing.DisplayOrder = model.DisplayOrder;
            existing.IsActive = model.IsActive;
            existing.UpdatedBy = model.UpdatedBy;
            existing.UpdatedDate = DateTime.Now;

            _context.Set<mst_NodalDetail>().Update(existing);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid guid)
        {
            if (guid == Guid.Empty) return false;

            var entity = await _context.Set<mst_NodalDetail>()
                .FirstOrDefaultAsync(x => x.Guid == guid);

            if (entity == null) return false;

            _context.Set<mst_NodalDetail>().Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ToggleStatusAsync(Guid guid)
        {
            if (guid == Guid.Empty) return false;

            var entity = await _context.Set<mst_NodalDetail>()
                .FirstOrDefaultAsync(x => x.Guid == guid);

            if (entity == null) return false;

            entity.IsActive = (entity.IsActive == 1) ? 0 : 1;
            entity.UpdatedDate = DateTime.Now;

            _context.Set<mst_NodalDetail>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
    }
