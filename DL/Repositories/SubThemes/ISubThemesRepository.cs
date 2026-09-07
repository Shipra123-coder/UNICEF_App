using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.SubThemes
{
    public interface ISubThemesRepository
    {
        Task<IEnumerable<mst_Sector>> GetAllAsync();
        Task<mst_Sector?> GetByIdAsync(int sectorId);
        Task<IEnumerable<mst_Pillar>> GetActivePillarsAsync();
        Task<bool> AddAsync(mst_Sector model);
        Task<bool> UpdateAsync(mst_Sector model);
        Task<bool> DeleteAsync(int sectorId);
        Task<bool> ToggleStatusAsync(int sectorId);
    }
}
