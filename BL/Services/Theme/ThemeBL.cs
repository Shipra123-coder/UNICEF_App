using DL.Repositories.ThemeRepository;
using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Theme
{
    public class ThemeBL : IThemeBL
    {
        private readonly IThemeRepository _themeRepo;

        public ThemeBL(IThemeRepository themeRepo)
        {
            _themeRepo = themeRepo;
        }

        public async Task<IEnumerable<mst_Pillar>> GetAllThemesAsync()
        {
            return await _themeRepo.GetAllAsync();
        }

        public async Task<mst_Pillar?> GetThemeByIdAsync(int id)
        {
            if (id <= 0) return null;
            return await _themeRepo.GetByIdAsync(id);
        }

        public async Task<(bool Success, string Message)> SaveThemeAsync(mst_Pillar model)
        {
            if (string.IsNullOrWhiteSpace(model.PillarName))
            {
                return (false, "Theme Name cannot be empty.");
            }

            if (model.PillarId == 0)
            {
                bool isAdded = await _themeRepo.AddAsync(model);
                return isAdded ? (true, "Theme added successfully.") : (false, "Failed to add theme.");
            }
            else
            {
                bool isUpdated = await _themeRepo.UpdateAsync(model);
                return isUpdated ? (true, "Theme updated successfully.") : (false, "Failed to update theme.");
            }
        }

        public async Task<(bool Success, string Message)> DeleteThemeAsync(int id)
        {
            if (id <= 0) return (false, "Invalid Theme ID.");
            bool isDeleted = await _themeRepo.DeleteAsync(id);
            return isDeleted ? (true, "Theme deleted successfully.") : (false, "Failed to delete theme.");
        }

        public async Task<(bool Success, string Message)> ToggleThemeStatusAsync(int id)
        {
            if (id <= 0) return (false, "Invalid Theme ID.");
            bool isToggled = await _themeRepo.ToggleStatusAsync(id);
            return isToggled ? (true, "Status updated successfully.") : (false, "Failed to update status.");
        }
    }
}
