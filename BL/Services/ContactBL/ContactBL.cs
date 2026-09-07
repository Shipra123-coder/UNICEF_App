using DL.Repositories.ContactRepository;
using MO.Entities;
using MO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.ContactBL
{
    public class ContactBL : IContactBL
    {
        private readonly IContactRepository _contactRepo;

        public ContactBL(IContactRepository contactRepo)
        {
            _contactRepo = contactRepo;
        }

        public async Task<IEnumerable<mst_ContactMaster>> GetAllContactsAsync()
        {
            return await _contactRepo.GetAllAsync();
        }

        public async Task<IEnumerable<mst_ContactMaster>> GetContactsByLevelAsync(int contactLevel)
        {
            return await _contactRepo.GetContactsByLevelAsync(contactLevel);
        }

        public async Task<mst_ContactMaster> GetContactByGuidAsync(Guid guid)
        {
            if (guid == Guid.Empty) return null;
            return await _contactRepo.GetByIdAsync(guid);
        }

        public async Task<bool> SaveContactAsync(mst_ContactMaster model)
        {
            if (model == null) return false;

            if (model.ContactId > 0)
            {
                // Update mode
                return await _contactRepo.UpdateAsync(model);
            }
            else
            {
                // Add mode: default image set karein agar empty ho
                if (string.IsNullOrWhiteSpace(model.PhotoUrl))
                {
                    model.PhotoUrl = "/uploads/contacts/default-officer.png";
                }

                return await _contactRepo.AddAsync(model);
            }
        }

        public async Task<bool> DeleteContactAsync(Guid guid)
        {
            if (guid == Guid.Empty) return false;
            return await _contactRepo.DeleteAsync(guid);
        }

        public async Task<bool> ToggleStatusAsync(Guid guid)
        {
            if (guid == Guid.Empty) return false;
            return await _contactRepo.ToggleStatusAsync(guid);
        }
    }
}
