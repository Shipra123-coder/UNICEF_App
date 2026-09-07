using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.ThemeRepository
{
    public interface IThemeRepository
    {
        Task<IEnumerable<mst_Pillar>> GetAllAsync();
        Task<mst_Pillar?> GetByIdAsync(int id);
        Task<bool> AddAsync(mst_Pillar model);
        Task<bool> UpdateAsync(mst_Pillar model);
        Task<bool> DeleteAsync(int id);
        Task<bool> ToggleStatusAsync(int id);
    }
}
