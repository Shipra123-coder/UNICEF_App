using DL.Repositories.Department;
using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Department
{
    public class DepartmentBL : IDepartmentBL
    {
        private readonly IDepartmentRepository _repository;

        public DepartmentBL(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<mst_Departments>> GetAllDepartmentsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<mst_Departments> GetDepartmentByIdAsync(Guid guid)
        {
            if (guid == Guid.Empty) return null;
            return await _repository.GetByIdAsync(guid);
        }

        public async Task<(bool Success, string Message)> SaveDepartmentAsync(mst_Departments model)
        {
            if (string.IsNullOrWhiteSpace(model.DepartmentName) || string.IsNullOrWhiteSpace(model.DepartmentCode))
            {
                return (false, "Department Name and Code are mandatory.");
            }

            if (model.DepartmentId == 0)
            {
                bool isAdded = await _repository.AddAsync(model);
                return isAdded ? (true, "Department registered successfully.") : (false, "Failed to register department.");
            }
            else
            {
                bool isUpdated = await _repository.UpdateAsync(model);
                return isUpdated ? (true, "Department updated successfully.") : (false, "Failed to update department.");
            }
        }

        public async Task<(bool Success, string Message)> DeleteDepartmentAsync(Guid guid)
        {
            if (guid == Guid.Empty) return (false, "Invalid Department ID.");
            bool isDeleted = await _repository.DeleteAsync(guid);
            return isDeleted ? (true, "Department deleted successfully.") : (false, "Failed to delete department.");
        }

        public async Task<(bool Success, string Message)> ToggleDepartmentStatusAsync(Guid guid)
        {
            if (guid == Guid.Empty) return (false, "Invalid Department ID.");
            bool isUpdated = await _repository.ToggleStatusAsync(guid);
            return isUpdated ? (true, "Status updated successfully.") : (false, "Failed to update status.");
        }
    }
}
