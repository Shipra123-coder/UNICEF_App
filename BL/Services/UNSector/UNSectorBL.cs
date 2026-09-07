using DL.Repositories.UNSector;
using MO.Entities;


namespace BL.Services.UNSector
{
    public class UNSectorBL : IUNSectorBL
    {
        private readonly IUNSectorRepository _repo;

        public UNSectorBL(IUNSectorRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<mst_UNSector>> GetAllUNSectorsAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<mst_UNSector?> GetUNSectorByIdAsync(long id)
        {
            if (id <= 0) return null;
            return await _repo.GetByIdAsync(id);
        }

        public async Task<(bool Success, string Message)> SaveUNSectorAsync(mst_UNSector model)
        {
            if (string.IsNullOrWhiteSpace(model.UNSectorName))
            {
                return (false, "UN Sector Name is mandatory.");
            }

            if (string.IsNullOrWhiteSpace(model.UNSectorCode))
            {
                return (false, "UN Sector Code is mandatory.");
            }

            if (model.UNSectorId == 0)
            {
                bool isAdded = await _repo.AddAsync(model);
                return isAdded ? (true, "UN Sector created successfully.") : (false, "Failed to create UN Sector.");
            }
            else
            {
                bool isUpdated = await _repo.UpdateAsync(model);
                return isUpdated ? (true, "UN Sector updated successfully.") : (false, "Failed to update UN Sector.");
            }
        }

        public async Task<(bool Success, string Message)> DeleteUNSectorAsync(long id)
        {
            if (id <= 0) return (false, "Invalid UN Sector ID.");
            bool isDeleted = await _repo.DeleteAsync(id);
            return isDeleted ? (true, "UN Sector deleted successfully.") : (false, "Failed to delete UN Sector.");
        }

        public async Task<(bool Success, string Message)> ToggleUNSectorStatusAsync(long id)
        {
            if (id <= 0) return (false, "Invalid UN Sector ID.");
            bool isToggled = await _repo.ToggleStatusAsync(id);
            return isToggled ? (true, "Status updated successfully.") : (false, "Failed to update status.");
        }
    }
}
