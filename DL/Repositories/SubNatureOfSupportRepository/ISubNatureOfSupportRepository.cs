using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.SubNatureOfSupportRepository
{
    public interface ISubNatureOfSupportRepository
    {
        Task<IEnumerable<mst_SubNatureOfSupport>> GetAllAsync();
        Task<mst_SubNatureOfSupport?> GetByIdAsync(long id);
        Task<IEnumerable<mst_NatureOfSupport>> GetActiveNatureOfSupportsAsync();
        Task<bool> AddAsync(mst_SubNatureOfSupport model);
        Task<bool> UpdateAsync(mst_SubNatureOfSupport model);
        Task<bool> DeleteAsync(long id);
        Task<bool> ToggleStatusAsync(long id);
    }
}
