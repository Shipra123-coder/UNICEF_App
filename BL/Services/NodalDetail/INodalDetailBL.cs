using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.NodalDetail
{
    public interface INodalDetailBL
    {
        Task<IEnumerable<mst_NodalDetail>> GetAllAsync();
        Task<mst_NodalDetail> GetByGuidAsync(Guid guid);
        Task<bool> SaveAsync(mst_NodalDetail model);
        Task<bool> DeleteAsync(Guid guid);
        Task<bool> ToggleStatusAsync(Guid guid);
    }
}
