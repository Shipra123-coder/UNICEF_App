using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.MenuRepository
{
    public interface IMenuRepository
    {
        Task<IEnumerable<m_Menu>> GetAllAsync();
        Task<m_Menu?> GetByGuidAsync(Guid guid);
        Task<IEnumerable<m_Menu>> GetParentMenusAsync();
        Task<bool> AddAsync(m_Menu model);
        Task<bool> UpdateAsync(m_Menu model);
        Task<bool> DeleteAsync(Guid guid);
        Task<bool> ToggleStatusAsync(Guid guid);
    }
}
