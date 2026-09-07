using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Theme
{
    public interface IThemeBL
    {
        Task<IEnumerable<mst_Pillar>> GetAllThemesAsync();
        Task<mst_Pillar?> GetThemeByIdAsync(int id);
        Task<(bool Success, string Message)> SaveThemeAsync(mst_Pillar model);
        Task<(bool Success, string Message)> DeleteThemeAsync(int id);
        Task<(bool Success, string Message)> ToggleThemeStatusAsync(int id);
    }
}
