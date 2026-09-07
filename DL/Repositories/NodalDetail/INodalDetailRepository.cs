using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.NodalDetail
{
    public interface INodalDetailRepository
    {
        Task<IEnumerable<mst_NodalDetail>> GetAllAsync();
        Task<mst_NodalDetail> GetByIdAsync(Guid guid);
        Task<bool> AddAsync(mst_NodalDetail model);
        Task<bool> UpdateAsync(mst_NodalDetail model);
        Task<bool> DeleteAsync(Guid guid);
        Task<bool> ToggleStatusAsync(Guid guid);
    }
}
