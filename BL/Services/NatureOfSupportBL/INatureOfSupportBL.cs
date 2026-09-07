using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.NatureOfSupportBL
{
    public interface INatureOfSupportBL
    {
        Task<IEnumerable<mst_NatureOfSupport>> GetAllAsync();
        Task<mst_NatureOfSupport?> GetByIdAsync(long id);
        Task<(bool Success, string Message)> SaveAsync(mst_NatureOfSupport model);
        Task<(bool Success, string Message)> DeleteAsync(long id);
        Task<(bool Success, string Message)> ToggleStatusAsync(long id);
    }
}
