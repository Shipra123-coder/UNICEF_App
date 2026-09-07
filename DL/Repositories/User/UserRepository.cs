using DL.Data;
using Microsoft.EntityFrameworkCore;
using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Dropdown

        public async Task<List<m_group>> GetActiveGroupsAsync()
        {
            return await _context.m_group.Where(x => x.IsActive == 1).ToListAsync();
        }

        public async Task<List<m_UserLevel>> GetUserLevelsAsync()
        {
            return await _context.m_UserLevel.ToListAsync();
        }

        public async Task<List<mst_Agency>> GetActiveAgenciesAsync()
        {
            return await _context.mst_Agency.Where(x => x.Status == 1).ToListAsync();
        }

        public async Task<List<mst_Departments>> GetActiveDepartmentAsync()
        {
            return await _context.mst_Departments.Where(x => x.Status == 1).ToListAsync();
        }
        #endregion

        #region User Add,SSO Exist, List

        public async Task<bool> AddUserAsync(Login_User user)
        {
            await _context.Login_User.AddAsync(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsSSOExistsAsync(string ssoId)
        {
            return await _context.Login_User.AnyAsync(x => x.SSOID.ToLower() == ssoId.ToLower());
        }
        public async Task<IEnumerable<Login_User>> GetAllUsersAsync()
        {
            // AsNoTracking() read-only performance ko fast karta hai
            return await _context.Login_User
                .AsNoTracking()
                .OrderByDescending(u => u.CreatedDate)
                .ToListAsync();
        }

        public async Task<Login_User?> GetUserByIdAsync(long id)
        {
            return await _context.Login_User.FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<bool> UpdateUserAsync(Login_User user)
        {
            _context.Login_User.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        // Check duplicate SSOID excluding the current user ID
        public async Task<bool> IsSSOExistsForOtherUserAsync(string ssoid, long userId)
        {
            return await _context.Login_User
                .AnyAsync(x => x.SSOID.ToLower() == ssoid.ToLower().Trim() && x.UserId != userId);
        }

        #endregion
    }
}
