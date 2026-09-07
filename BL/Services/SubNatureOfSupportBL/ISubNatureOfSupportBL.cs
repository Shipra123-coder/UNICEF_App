using Microsoft.AspNetCore.Mvc.Rendering;
using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.SubNatureOfSupportBL
{
    public interface ISubNatureOfSupportBL
    {
        Task<IEnumerable<mst_SubNatureOfSupport>> GetAllAsync();
        Task<mst_SubNatureOfSupport?> GetByIdAsync(long id);
        Task<IEnumerable<SelectListItem>> GetNatureOfSupportDropdownAsync();
        Task<(bool Success, string Message)> SaveAsync(mst_SubNatureOfSupport model);
        Task<(bool Success, string Message)> DeleteAsync(long id);
        Task<(bool Success, string Message)> ToggleStatusAsync(long id);
    }
}

