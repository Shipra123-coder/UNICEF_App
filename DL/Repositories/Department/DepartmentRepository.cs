using DL.Data;
using Microsoft.EntityFrameworkCore;
using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.Department
{ 
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<mst_Departments>> GetAllAsync()
        {
            return await _context.Set<mst_Departments>()
                .AsNoTracking()
                .OrderByDescending(d => d.DepartmentId)
                .ToListAsync();
        }

        public async Task<mst_Departments> GetByIdAsync(Guid guid)
        {
            if (guid == Guid.Empty)
            {
                return null;
            }

            return await _context.Set<mst_Departments>()
                                 .AsNoTracking() // Read-only query ke liye performance improve karega
                                 .FirstOrDefaultAsync(x => x.Guid == guid);
        }

        public async Task<bool> AddAsync(mst_Departments model)
        {
            model.Guid = Guid.NewGuid();
            model.CreatedDate = DateTime.Now;
            model.Status ??= 1;
            await _context.Set<mst_Departments>().AddAsync(model);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(mst_Departments model)
        {
            var existing = await _context.Set<mst_Departments>().FindAsync(model.DepartmentId);
            if (existing == null) return false;

            existing.DepartmentName = model.DepartmentName;
            existing.DepartmentCode = model.DepartmentCode;
            existing.Description = model.Description;
            existing.HeadOfDepartment = model.HeadOfDepartment;
            existing.Email = model.Email;
            existing.Phone = model.Phone;
            existing.Address = model.Address;
            existing.Status = model.Status;
            existing.LogoUrl = model.LogoUrl; // Hidden field se purana ya naya URL update ho jayega
            existing.UpdatedBy = model.UpdatedBy;
            existing.UpdatedDate = DateTime.Now;

            _context.Set<mst_Departments>().Update(existing);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid guid)
        {
            if (guid == Guid.Empty) return false;

            var entity = await _context.Set<mst_Departments>()
                                       .FirstOrDefaultAsync(x => x.Guid == guid);

            if (entity == null) return false;

            _context.Set<mst_Departments>().Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ToggleStatusAsync(Guid guid)
        {
            if (guid == Guid.Empty) return false;

            var entity = await _context.Set<mst_Departments>().FirstOrDefaultAsync(x => x.Guid == guid); ;
            if (entity == null) return false;

            entity.Status = (entity.Status == 1) ? 0 : 1;
            entity.UpdatedDate = DateTime.Now;
            _context.Set<mst_Departments>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
