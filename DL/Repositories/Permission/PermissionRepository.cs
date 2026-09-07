using DL.Data;
using Microsoft.EntityFrameworkCore;
using MO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Repositories.Permission
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext _db;

        public PermissionRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> HasMenuAccessAsync(int groupId, string controller, string action, PermissionAction permAction)
        {
            // Menu table aur Permission table ka relation query karein
            var permission = await (from m in _db.m_Menu
                                    join p in _db.m_MenuPermission on m.MenuId equals p.MenuId
                                    where p.GroupId == groupId
                                          && m.Controller.ToLower() == controller.ToLower()
                                          && (m.ActionName.ToLower() == action.ToLower() || m.ActionName == "#" || action == "Index")
                                          && m.IsActive == 1
                                    select p).FirstOrDefaultAsync();

            if (permission == null)
                return false;

            return permAction switch
            {
                PermissionAction.List => permission.CanList,
                PermissionAction.Add => permission.CanAdd,
                PermissionAction.Edit => permission.CanEdit,
                PermissionAction.Delete => permission.CanDelete,
                PermissionAction.ActiveDeactive => permission.CanActiveDeactive,
                _ => false
            };
        }
    }
}
