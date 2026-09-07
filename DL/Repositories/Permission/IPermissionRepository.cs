using MO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.Permission
{
    public interface IPermissionRepository
    {
        Task<bool> HasMenuAccessAsync(int groupId, string controller, string action, PermissionAction permAction);
    }
}
