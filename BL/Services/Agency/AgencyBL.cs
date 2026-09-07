using DL.Repositories.Agency;
using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Agency
{
    public class AgencyBL : IAgencyBL
    {
        private readonly IAgencyRepository _agencyRepo;

        public AgencyBL(IAgencyRepository agencyRepo)
        {
            _agencyRepo = agencyRepo;
        }

        public async Task<IEnumerable<mst_Agency>> GetAllAgenciesAsync()
        {
            return await _agencyRepo.GetAllAsync();
        }

        public async Task<mst_Agency?> GetAgencyByGuidAsync(Guid guid)
        {
            if (guid == Guid.Empty) return null;
            return await _agencyRepo.GetByGuidAsync(guid);
        }

        public async Task<(bool Success, string Message)> SaveAgencyAsync(mst_Agency model)
        {
            if (string.IsNullOrWhiteSpace(model.AgencyName) || string.IsNullOrWhiteSpace(model.AgencyCode))
            {
                return (false, "Agency Name and Agency Code are mandatory.");
            }

            if (!model.Guid.HasValue || model.Guid == Guid.Empty)
            {
                bool isAdded = await _agencyRepo.AddAsync(model);
                return isAdded ? (true, "Agency added successfully.") : (false, "Failed to add agency.");
            }
            else
            {
                bool isUpdated = await _agencyRepo.UpdateAsync(model);
                return isUpdated ? (true, "Agency updated successfully.") : (false, "Failed to update agency.");
            }
        }

        public async Task<(bool Success, string Message)> DeleteAgencyAsync(Guid guid)
        {
            if (guid == Guid.Empty) return (false, "Invalid Agency record.");
            bool isDeleted = await _agencyRepo.DeleteAsync(guid);
            return isDeleted ? (true, "Agency deleted successfully.") : (false, "Failed to delete agency.");
        }

        public async Task<(bool Success, string Message)> ToggleAgencyStatusAsync(Guid guid)
        {
            if (guid == Guid.Empty) return (false, "Invalid Agency record.");
            bool isToggled = await _agencyRepo.ToggleStatusAsync(guid);
            return isToggled ? (true, "Status updated successfully.") : (false, "Failed to update status.");
        }
    }
}
