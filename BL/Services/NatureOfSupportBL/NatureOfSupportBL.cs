using MO.Entities;
using MO.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.NatureOfSupportBL
{
    public class NatureOfSupportBL : INatureOfSupportBL
    {
        private readonly INatureOfSupportRepository _repo;

        public NatureOfSupportBL(INatureOfSupportRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<mst_NatureOfSupport>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<mst_NatureOfSupport?> GetByIdAsync(long id)
        {
            if (id <= 0) return null;
            return await _repo.GetByIdAsync(id);
        }

        public async Task<(bool Success, string Message)> SaveAsync(mst_NatureOfSupport model)
        {
            if (string.IsNullOrWhiteSpace(model.NatureSupportName))
            {
                return (false, "Nature of Support Name is required.");
            }

            if (model.NatureSupportId == 0)
            {
                bool isAdded = await _repo.AddAsync(model);
                return isAdded ? (true, "Record added successfully.") : (false, "Failed to add record.");
            }
            else
            {
                bool isUpdated = await _repo.UpdateAsync(model);
                return isUpdated ? (true, "Record updated successfully.") : (false, "Failed to update record.");
            }
        }

        public async Task<(bool Success, string Message)> DeleteAsync(long id)
        {
            if (id <= 0) return (false, "Invalid ID.");
            bool isDeleted = await _repo.DeleteAsync(id);
            return isDeleted ? (true, "Record deleted successfully.") : (false, "Failed to delete record.");
        }

        public async Task<(bool Success, string Message)> ToggleStatusAsync(long id)
        {
            if (id <= 0) return (false, "Invalid ID.");
            bool isToggled = await _repo.ToggleStatusAsync(id);
            return isToggled ? (true, "Status updated successfully.") : (false, "Failed to update status.");
        }
    }
}
