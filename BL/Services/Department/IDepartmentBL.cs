using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Department
{
    public interface IDepartmentBL
    {
        Task<IEnumerable<mst_Departments>> GetAllDepartmentsAsync();
        Task<mst_Departments> GetDepartmentByIdAsync(Guid guid);
        Task<(bool Success, string Message)> SaveDepartmentAsync(mst_Departments model);
        Task<(bool Success, string Message)> DeleteDepartmentAsync(Guid guid);
        Task<(bool Success, string Message)> ToggleDepartmentStatusAsync(Guid guid);
    }
}
