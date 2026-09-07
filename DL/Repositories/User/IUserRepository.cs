using Microsoft.EntityFrameworkCore;
using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.User
{
    public interface IUserRepository
    {
        Task<List<m_group>> GetActiveGroupsAsync();
        Task<List<m_UserLevel>> GetUserLevelsAsync();
        Task<List<mst_Agency>> GetActiveAgenciesAsync();
        Task<List<mst_Departments>> GetActiveDepartmentAsync();
        Task<bool> AddUserAsync(Login_User user);
        Task<bool> IsSSOExistsAsync(string ssoId);

        Task<IEnumerable<Login_User>> GetAllUsersAsync();
        Task<Login_User?> GetUserByIdAsync(long id);
        Task<bool> UpdateUserAsync(Login_User user);
        Task<bool> IsSSOExistsForOtherUserAsync(string ssoid, long userId);
    }
}
