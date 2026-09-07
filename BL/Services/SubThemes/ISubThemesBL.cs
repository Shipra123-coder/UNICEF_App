using Microsoft.AspNetCore.Mvc.Rendering;
using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BL.Services.SubThemes
{
    public interface ISubThemesBL
    {
        Task<IEnumerable<mst_Sector>> GetAllSectorsAsync();
        Task<mst_Sector?> GetSectorByIdAsync(int sectorId);
        Task<IEnumerable<SelectListItem>> GetPillarDropdownAsync();
        Task<(bool Success, string Message)> SaveSectorAsync(mst_Sector model);
        Task<(bool Success, string Message)> DeleteSectorAsync(int sectorId);
        Task<(bool Success, string Message)> ToggleSectorStatusAsync(int sectorId);
    }
}
