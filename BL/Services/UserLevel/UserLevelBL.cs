using DL.Repositories.UserLevel;
using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.UserLevel
{
    public class UserLevelBL : IUserLevelBL
    {
        private readonly IUserLevelRepository _repository;

        public UserLevelBL(IUserLevelRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<m_UserLevel>> GetAllUserLevelsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<m_UserLevel> GetUserLevelByIdAsync(int id)
        {
            if (id <= 0) return null;
            return await _repository.GetByIdAsync(id);
        }

        public async Task<(bool Success, string Message)> SaveUserLevelAsync(m_UserLevel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                return (false, "User Level Name cannot be empty.");
            }

            if (model.Id == 0)
            {
                bool isAdded = await _repository.AddAsync(model);
                return isAdded ? (true, "User Level created successfully.") : (false, "Failed to create User Level.");
            }
            else
            {
                bool isUpdated = await _repository.UpdateAsync(model);
                return isUpdated ? (true, "User Level updated successfully.") : (false, "Failed to update User Level.");
            }
        }

        public async Task<(bool Success, string Message)> DeleteUserLevelAsync(int id)
        {
            if (id <= 0) return (false, "Invalid ID.");
            bool isDeleted = await _repository.DeleteAsync(id);
            return isDeleted ? (true, "User Level deleted successfully.") : (false, "Failed to delete User Level.");
        }
    }
}
