using MO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Permission
{
    public interface IPermissionBL
    {
        Task<bool> HasAccessAsync(int groupId, string controller, string action, PermissionAction actionType);
        
    }
}
