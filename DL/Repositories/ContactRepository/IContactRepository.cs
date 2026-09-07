using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.ContactRepository
{
    public interface IContactRepository
    {
        Task<IEnumerable<mst_ContactMaster>> GetAllAsync();
        Task<IEnumerable<mst_ContactMaster>> GetContactsByLevelAsync(int contactLevel);
        Task<mst_ContactMaster> GetByIdAsync(Guid guid);
        Task<bool> AddAsync(mst_ContactMaster model);
        Task<bool> UpdateAsync(mst_ContactMaster model);
        Task<bool> DeleteAsync(Guid guid);
        Task<bool> ToggleStatusAsync(Guid guid);
    }
}
