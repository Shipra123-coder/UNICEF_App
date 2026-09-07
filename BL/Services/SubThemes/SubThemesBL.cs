using DL.Repositories.SubThemes;
using Microsoft.AspNetCore.Mvc.Rendering;
using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BL.Services.SubThemes
{
    public class SubThemesBL : ISubThemesBL
    {
        private readonly ISubThemesRepository _sectorRepo;

        public SubThemesBL(ISubThemesRepository sectorRepo)
        {
            _sectorRepo = sectorRepo;
        }

        public async Task<IEnumerable<mst_Sector>> GetAllSectorsAsync()
        {
            return await _sectorRepo.GetAllAsync();
        }

        public async Task<mst_Sector?> GetSectorByIdAsync(int sectorId)
        {
            if (sectorId <= 0) return null;
            return await _sectorRepo.GetByIdAsync(sectorId);
        }

        public async Task<IEnumerable<SelectListItem>> GetPillarDropdownAsync()
        {
            var pillars = await _sectorRepo.GetActivePillarsAsync();
            return pillars.Select(p => new SelectListItem
            {
                Value = p.PillarId.ToString(),
                Text = $"{p.PillarId}. {p.PillarName}"
            }).ToList();
        }

        public async Task<(bool Success, string Message)> SaveSectorAsync(mst_Sector model)
        {
            if (model.PillarId <= 0)
            {
                return (false, "Please select a valid Pillar.");
            }

            if (string.IsNullOrWhiteSpace(model.SectorName))
            {
                return (false, "Sector Name cannot be empty.");
            }

            if (model.SectorId == 0)
            {
                bool isAdded = await _sectorRepo.AddAsync(model);
                return isAdded ? (true, "Sector added successfully.") : (false, "Failed to add sector.");
            }
            else
            {
                bool isUpdated = await _sectorRepo.UpdateAsync(model);
                return isUpdated ? (true, "Sector updated successfully.") : (false, "Failed to update sector.");
            }
        }

        public async Task<(bool Success, string Message)> DeleteSectorAsync(int sectorId)
        {
            if (sectorId <= 0) return (false, "Invalid Sector ID.");
            bool isDeleted = await _sectorRepo.DeleteAsync(sectorId);
            return isDeleted ? (true, "Sector deleted successfully.") : (false, "Failed to delete sector.");
        }

        public async Task<(bool Success, string Message)> ToggleSectorStatusAsync(int sectorId)
        {
            if (sectorId <= 0) return (false, "Invalid Sector ID.");
            bool isToggled = await _sectorRepo.ToggleStatusAsync(sectorId);
            return isToggled ? (true, "Status changed successfully.") : (false, "Failed to change status.");
        }
    }

}
