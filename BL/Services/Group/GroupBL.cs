using DL.Repositories.Group;
using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Group
{
    public class GroupBL : IGroupBL
    {
        private readonly IGroupRepository _groupRepo;

        public GroupBL(IGroupRepository groupRepo)
        {
            _groupRepo = groupRepo;
        }

        public async Task<IEnumerable<m_group>> GetAllGroupsAsync()
        {
            return await _groupRepo.GetAllAsync();
        }

        public async Task<m_group?> GetGroupByGuidAsync(Guid guid)
        {
            if (guid == Guid.Empty) return null;
            return await _groupRepo.GetByGuidAsync(guid);
        }

        public async Task<(bool Success, string Message)> SaveGroupAsync(m_group model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                return (false, "Group Name is mandatory.");
            }

            if (!model.Guid.HasValue || model.Guid == Guid.Empty)
            {
                bool isAdded = await _groupRepo.AddAsync(model);
                return isAdded ? (true, "Group added successfully.") : (false, "Failed to add group.");
            }
            else
            {
                bool isUpdated = await _groupRepo.UpdateAsync(model);
                return isUpdated ? (true, "Group updated successfully.") : (false, "Failed to update group.");
            }
        }

        public async Task<(bool Success, string Message)> DeleteGroupAsync(Guid guid)
        {
            if (guid == Guid.Empty) return (false, "Invalid Group identifier.");
            bool isDeleted = await _groupRepo.DeleteAsync(guid);
            return isDeleted ? (true, "Group deleted successfully.") : (false, "Failed to delete group.");
        }

        public async Task<(bool Success, string Message)> ToggleGroupStatusAsync(Guid guid)
        {
            if (guid == Guid.Empty) return (false, "Invalid Group identifier.");
            bool isToggled = await _groupRepo.ToggleStatusAsync(guid);
            return isToggled ? (true, "Status updated successfully.") : (false, "Failed to update status.");
        }
    }
}
