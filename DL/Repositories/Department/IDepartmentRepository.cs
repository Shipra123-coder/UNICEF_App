using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.Department
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<mst_Departments>> GetAllAsync();
        Task<mst_Departments> GetByIdAsync(Guid guid);
        Task<bool> AddAsync(mst_Departments model);
        Task<bool> UpdateAsync(mst_Departments model);
        Task<bool> DeleteAsync(Guid guid);
        Task<bool> ToggleStatusAsync(Guid guid);
    }
}
