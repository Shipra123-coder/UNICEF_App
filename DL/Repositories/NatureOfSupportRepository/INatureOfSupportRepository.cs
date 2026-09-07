using System.Collections.Generic;
using System.Threading.Tasks;
using MO.Entities;

namespace MO.Repositories
{
    public interface INatureOfSupportRepository
    {
        Task<IEnumerable<mst_NatureOfSupport>> GetAllAsync();
        Task<mst_NatureOfSupport?> GetByIdAsync(long id);
        Task<bool> AddAsync(mst_NatureOfSupport model);
        Task<bool> UpdateAsync(mst_NatureOfSupport model);
        Task<bool> DeleteAsync(long id);
        Task<bool> ToggleStatusAsync(long id);
    }
}