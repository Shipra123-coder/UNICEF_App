using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.UserLevel
{
    public interface IUserLevelBL
    {
        Task<IEnumerable<m_UserLevel>> GetAllUserLevelsAsync();
        Task<m_UserLevel> GetUserLevelByIdAsync(int id);
        Task<(bool Success, string Message)> SaveUserLevelAsync(m_UserLevel model);
        Task<(bool Success, string Message)> DeleteUserLevelAsync(int id);
    }
}
