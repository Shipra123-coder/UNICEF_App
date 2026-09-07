using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.CMDetailsBL
{
    public interface ICMDetailsBL
    {
        Task<IEnumerable<mst_CMDetails>> GetAllAsync();
        Task<mst_CMDetails> GetByGuidAsync(Guid guid);
        Task<bool> SaveAsync(mst_CMDetails model);
        Task<bool> DeleteAsync(Guid guid);
        Task<bool> ToggleStatusAsync(Guid guid);
    }
}
