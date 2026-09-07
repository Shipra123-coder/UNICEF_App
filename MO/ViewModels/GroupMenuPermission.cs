using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.ViewModels
{
    public class GroupMenuPermission
    {
        public long MenuId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? MangalName { get; set; }
        public string? Controller { get; set; }
        public string? ActionName { get; set; }
        public string? Icon { get; set; }
        public bool IsSubMenu { get; set; }
        public long? MainMenuId { get; set; }
        public int? OrderNumber { get; set; }

        public long PermissionId { get; set; }
        public bool CanList { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanActiveDeactive { get; set; }
    }

    public class SaveGroupPermissionDto
    {
        public int GroupId { get; set; }
        public List<GroupMenuPermission> Permissions { get; set; } = new();
    }

    public class MenuPermissionViewModel
    {
        public int SelectedGroupId { get; set; }
        public List<GroupMenuPermission> MenuList { get; set; } = new();
    }
}
