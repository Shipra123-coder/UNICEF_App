using DL.Repositories.CMDetailsRepository;
using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.CMDetailsBL
{
    public class CMDetailsBL : ICMDetailsBL
    {
        private readonly ICMDetailsRepository _cmRepo;

        public CMDetailsBL(ICMDetailsRepository cmRepo)
        {
            _cmRepo = cmRepo;
        }

        public async Task<IEnumerable<mst_CMDetails>> GetAllAsync()
        {
            return await _cmRepo.GetAllAsync();
        }

        public async Task<mst_CMDetails> GetByGuidAsync(Guid guid)
        {
            if (guid == Guid.Empty) return null;
            return await _cmRepo.GetByIdAsync(guid);
        }

        public async Task<bool> SaveAsync(mst_CMDetails model)
        {
            if (model == null) return false;

            if (model.CMId > 0)
            {
                return await _cmRepo.UpdateAsync(model);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(model.PhotoUrl))
                {
                    model.PhotoUrl = "/images/default-officer.png";
                }
                return await _cmRepo.AddAsync(model);
            }
        }

        public async Task<bool> DeleteAsync(Guid guid)
        {
            if (guid == Guid.Empty) return false;
            return await _cmRepo.DeleteAsync(guid);
        }

        public async Task<bool> ToggleStatusAsync(Guid guid)
        {
            if (guid == Guid.Empty) return false;
            return await _cmRepo.ToggleStatusAsync(guid);
        }
    }
}
