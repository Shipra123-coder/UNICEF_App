using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.CMDetailsRepository
{
    public interface ICMDetailsRepository
    {
        Task<IEnumerable<mst_CMDetails>> GetAllAsync();
        Task<mst_CMDetails> GetByIdAsync(Guid guid);
        Task<bool> AddAsync(mst_CMDetails model);
        Task<bool> UpdateAsync(mst_CMDetails model);
        Task<bool> DeleteAsync(Guid guid);
        Task<bool> ToggleStatusAsync(Guid guid);
    }
}
