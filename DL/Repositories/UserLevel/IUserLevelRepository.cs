using MO.Entities;
using MO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.UserLevel
{
    public interface IUserLevelRepository
    {
        Task<IEnumerable<m_UserLevel>> GetAllAsync();
        Task<m_UserLevel> GetByIdAsync(int id);
        Task<bool> AddAsync(m_UserLevel model);
        Task<bool> UpdateAsync(m_UserLevel model);
        Task<bool> DeleteAsync(int id);
    }
}
