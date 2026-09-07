using DL.Repositories.NodalDetail;
using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.NodalDetail
{
    public class NodalDetailBL : INodalDetailBL
    {
        private readonly INodalDetailRepository _nodalRepo;

        public NodalDetailBL(INodalDetailRepository nodalRepo)
        {
            _nodalRepo = nodalRepo;
        }

        public async Task<IEnumerable<mst_NodalDetail>> GetAllAsync()
        {
            return await _nodalRepo.GetAllAsync();
        }

        public async Task<mst_NodalDetail> GetByGuidAsync(Guid guid)
        {
            if (guid == Guid.Empty) return null;
            return await _nodalRepo.GetByIdAsync(guid);
        }

        public async Task<bool> SaveAsync(mst_NodalDetail model)
        {
            if (model == null) return false;

            if (model.NodalId > 0)
            {
                return await _nodalRepo.UpdateAsync(model);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(model.PhotoUrl))
                {
                    model.PhotoUrl = "/images/default-officer.png";
                }
                return await _nodalRepo.AddAsync(model);
            }
        }

        public async Task<bool> DeleteAsync(Guid guid)
        {
            if (guid == Guid.Empty) return false;
            return await _nodalRepo.DeleteAsync(guid);
        }

        public async Task<bool> ToggleStatusAsync(Guid guid)
        {
            if (guid == Guid.Empty) return false;
            return await _nodalRepo.ToggleStatusAsync(guid);
        }
    }
}
