using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.UNSector
{
    public interface IUNSectorRepository
    {
        Task<IEnumerable<mst_UNSector>> GetAllAsync();
        Task<mst_UNSector?> GetByIdAsync(long id);
        Task<bool> AddAsync(mst_UNSector model);
        Task<bool> UpdateAsync(mst_UNSector model);
        Task<bool> DeleteAsync(long id);
        Task<bool> ToggleStatusAsync(long id);
    }
}
