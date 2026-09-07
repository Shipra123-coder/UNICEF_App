using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.Group
{
    public interface IGroupRepository
    {
        Task<IEnumerable<m_group>> GetAllAsync();
        Task<m_group?> GetByGuidAsync(Guid guid);
        Task<bool> AddAsync(m_group model);
        Task<bool> UpdateAsync(m_group model);
        Task<bool> DeleteAsync(Guid guid);
        Task<bool> ToggleStatusAsync(Guid guid);
    }
}
