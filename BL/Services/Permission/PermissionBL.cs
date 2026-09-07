using DL.Repositories.Permission;
using MO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Permission
{
    public class PermissionBL : IPermissionBL
    {
        private readonly IPermissionRepository _permissionRepo;

        public PermissionBL(IPermissionRepository permissionRepo)
        {
            _permissionRepo = permissionRepo;
        }

        public async Task<bool> HasAccessAsync(int groupId, string controller, string action, PermissionAction actionType)
        {
            // Super Admin bypass (Rule: GroupId 1 = SuperAdmin)
            if (groupId == 1) return true;

            if (string.IsNullOrWhiteSpace(controller) || string.IsNullOrWhiteSpace(action))
                return false;

            var perm = await _permissionRepo.HasMenuAccessAsync(groupId, controller, action, actionType);
            return perm;           
        }

      }
}
