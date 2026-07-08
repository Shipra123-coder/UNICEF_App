using MO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Common
{
    public interface IPermission
    {
        Task<MenuPermissionMO?> GetPagePermissionAsync(int groupId, string url);
    }
}
