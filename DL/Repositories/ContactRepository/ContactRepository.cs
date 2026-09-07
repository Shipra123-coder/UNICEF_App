using DL.Data;
using Microsoft.EntityFrameworkCore;
using MO.Entities;
using MO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.ContactRepository
{
    public class ContactRepository : IContactRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<mst_ContactMaster>> GetAllAsync()
        {
            return await _context.Set<mst_ContactMaster>()
                .AsNoTracking()
                .OrderBy(c => c.ContactLevel)
                .ThenBy(c => c.DisplayOrder)
                .ThenByDescending(c => c.ContactId)
                .ToListAsync();
        }

        public async Task<IEnumerable<mst_ContactMaster>> GetContactsByLevelAsync(int contactLevel)
        {
            // int ko Enum me cast karein
            var levelEnum = (ContactLevelEnum)contactLevel;

            return await _context.Set<mst_ContactMaster>()
                .AsNoTracking()
                .Where(x => x.ContactLevel == levelEnum && x.IsActive == 1)
                .OrderBy(x => x.DisplayOrder)
                .ThenByDescending(x => x.ContactId)
                .ToListAsync();
        }

        public async Task<mst_ContactMaster> GetByIdAsync(Guid guid)
        {
            if (guid == Guid.Empty)
            {
                return null;
            }

            return await _context.Set<mst_ContactMaster>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Guid == guid);
        }

        public async Task<bool> AddAsync(mst_ContactMaster model)
        {
            model.Guid = Guid.NewGuid();
            model.CreatedDate = DateTime.Now;
            model.IsActive = 1;

            await _context.Set<mst_ContactMaster>().AddAsync(model);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(mst_ContactMaster model)
        {
            var existing = await _context.Set<mst_ContactMaster>().FindAsync(model.ContactId);
            if (existing == null) return false;

            existing.ContactLevel = model.ContactLevel;
            existing.OfficerName = model.OfficerName;
            existing.Designation = model.Designation;
            existing.OfficeLandline = model.OfficeLandline;
            existing.OfficeIpNumber = model.OfficeIpNumber;
            existing.EmailId = model.EmailId;
            existing.PhotoUrl = model.PhotoUrl;
            existing.DisplayOrder = model.DisplayOrder;
            existing.IsActive = model.IsActive;
            existing.UpdatedBy = model.UpdatedBy;
            existing.UpdatedDate = DateTime.Now;

            _context.Set<mst_ContactMaster>().Update(existing);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid guid)
        {
            if (guid == Guid.Empty) return false;

            var entity = await _context.Set<mst_ContactMaster>()
                .FirstOrDefaultAsync(x => x.Guid == guid);

            if (entity == null) return false;

            _context.Set<mst_ContactMaster>().Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ToggleStatusAsync(Guid guid)
        {
            if (guid == Guid.Empty) return false;

            var entity = await _context.Set<mst_ContactMaster>()
                .FirstOrDefaultAsync(x => x.Guid == guid);

            if (entity == null) return false;

            entity.IsActive = (entity.IsActive == 1) ? 0 : 1;
            entity.UpdatedDate = DateTime.Now;

            _context.Set<mst_ContactMaster>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
    }
