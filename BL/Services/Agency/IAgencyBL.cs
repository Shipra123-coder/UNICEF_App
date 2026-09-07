using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Agency
{
    public interface IAgencyBL
    {
        Task<IEnumerable<mst_Agency>> GetAllAgenciesAsync();
        Task<mst_Agency?> GetAgencyByGuidAsync(Guid guid);
        Task<(bool Success, string Message)> SaveAgencyAsync(mst_Agency model);
        Task<(bool Success, string Message)> DeleteAgencyAsync(Guid guid);
        Task<(bool Success, string Message)> ToggleAgencyStatusAsync(Guid guid);
    }
}
