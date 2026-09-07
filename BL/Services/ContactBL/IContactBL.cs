using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.ContactBL
{
    public interface IContactBL
    {
        Task<IEnumerable<mst_ContactMaster>> GetAllContactsAsync();
        Task<IEnumerable<mst_ContactMaster>> GetContactsByLevelAsync(int contactLevel);
        Task<mst_ContactMaster> GetContactByGuidAsync(Guid guid);
        Task<bool> SaveContactAsync(mst_ContactMaster model);
        Task<bool> DeleteContactAsync(Guid guid);
        Task<bool> ToggleStatusAsync(Guid guid);
    }
}
