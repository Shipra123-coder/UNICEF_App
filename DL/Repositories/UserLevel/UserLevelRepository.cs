using DL.Data;
using Microsoft.EntityFrameworkCore;
using MO.Entities;
using MO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.UserLevel
{
    public class UserLevelRepository : IUserLevelRepository
    {
        private readonly ApplicationDbContext _context;

        public UserLevelRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<m_UserLevel>> GetAllAsync()
        {
            return await _context.m_UserLevel.AsNoTracking()
                .OrderByDescending(x => x.Id)
                .ToListAsync();
        }

        public async Task<m_UserLevel> GetByIdAsync(int id)
        {
            return await _context.m_UserLevel.FindAsync(id);
        }

        public async Task<bool> AddAsync(m_UserLevel model)
        {
            model.CreatedDate ??= DateTime.Now;
            await _context.m_UserLevel.AddAsync(model);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(m_UserLevel model)
        {
            var existing = await _context.m_UserLevel.FindAsync(model.Id);
            if (existing == null) return false;

            existing.Name = model.Name;
            _context.m_UserLevel.Update(existing);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.m_UserLevel.FindAsync(id);
            if (entity == null) return false;

            _context.m_UserLevel.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
