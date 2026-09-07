using DL.Data;
using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.Agency
{
    public interface IAgencyRepository
    {
        Task<IEnumerable<mst_Agency>> GetAllAsync();
        Task<mst_Agency?> GetByGuidAsync(Guid guid);
        Task<bool> AddAsync(mst_Agency model);
        Task<bool> UpdateAsync(mst_Agency model);
        Task<bool> DeleteAsync(Guid guid);
        Task<bool> ToggleStatusAsync(Guid guid);
    }
}
