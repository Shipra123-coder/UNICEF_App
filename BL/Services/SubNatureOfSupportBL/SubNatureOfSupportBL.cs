using DL.Repositories.SubNatureOfSupportRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.SubNatureOfSupportBL
{
    public class SubNatureOfSupportBL : ISubNatureOfSupportBL
    {
        private readonly ISubNatureOfSupportRepository _repo;

        public SubNatureOfSupportBL(ISubNatureOfSupportRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<mst_SubNatureOfSupport>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<mst_SubNatureOfSupport?> GetByIdAsync(long id)
        {
            if (id <= 0) return null;
            return await _repo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<SelectListItem>> GetNatureOfSupportDropdownAsync()
        {
            var list = await _repo.GetActiveNatureOfSupportsAsync();
            return list.Select(n => new SelectListItem
            {
                Value = n.NatureSupportId.ToString(),
                Text = n.NatureSupportName
            }).ToList();
        }

        public async Task<(bool Success, string Message)> SaveAsync(mst_SubNatureOfSupport model)
        {
            if (model.NatureSupportId <= 0)
            {
                return (false, "Please select a valid Nature of Support.");
            }

            if (string.IsNullOrWhiteSpace(model.SupportDetailName))
            {
                return (false, "Support Detail Name cannot be empty.");
            }

            if (model.SubNatureOfSupportId == 0)
            {
                bool isAdded = await _repo.AddAsync(model);
                return isAdded ? (true, "Sub Nature of Support added successfully.") : (false, "Failed to add record.");
            }
            else
            {
                bool isUpdated = await _repo.UpdateAsync(model);
                return isUpdated ? (true, "Sub Nature of Support updated successfully.") : (false, "Failed to update record.");
            }
        }

        public async Task<(bool Success, string Message)> DeleteAsync(long id)
        {
            if (id <= 0) return (false, "Invalid ID.");
            bool isDeleted = await _repo.DeleteAsync(id);
            return isDeleted ? (true, "Sub Nature of Support deleted successfully.") : (false, "Failed to delete record.");
        }

        public async Task<(bool Success, string Message)> ToggleStatusAsync(long id)
        {
            if (id <= 0) return (false, "Invalid ID.");
            bool isToggled = await _repo.ToggleStatusAsync(id);
            return isToggled ? (true, "Status updated successfully.") : (false, "Failed to update status.");
        }
    }
}
