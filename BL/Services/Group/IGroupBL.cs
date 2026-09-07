using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Group
{
    public interface IGroupBL
    {
        Task<IEnumerable<m_group>> GetAllGroupsAsync();
        Task<m_group?> GetGroupByGuidAsync(Guid guid);
        Task<(bool Success, string Message)> SaveGroupAsync(m_group model);
        Task<(bool Success, string Message)> DeleteGroupAsync(Guid guid);
        Task<(bool Success, string Message)> ToggleGroupStatusAsync(Guid guid);
    }
}
