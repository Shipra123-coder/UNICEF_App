using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.MenuMaster
{
    public class MenuMO
    {
        public long MenuId { get; set; }
        public string Guid { get; set; }
        public long? HeaderMenu { get; set; }
        public string Name { get; set; }
        public string MangalName { get; set; }
        public string Controller { get; set; }
        public string ActionName { get; set; }
        public int? IsSubMenu { get; set; }
        public int? MainMenuId { get; set; }
        public int OrderNumber { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; }
        public string BtnSave { get; set; } // add this
    }
    public class MenuList
    {
        public int SrNo { get; set; }
        public long MenuId { get; set; }
        public string MenuName { get; set; }
        public string MangalName { get; set; }
        public string HeaderMenu { get; set; }
        public string Controller { get; set; }
        public string ActionName { get; set; }
        public string OrderNumber { get; set; }
        public string Icon { get; set; }
        public string IsActive { get; set; }
        public string Action { get; set; }
    }

    public class MenuItem
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string IconClass { get; set; }
        public int OrderNumber { get; set; }
        public bool isChecked { get; set; }
        public string? url { get; set; }

        public bool? CanAdd { get; set; }
        public bool? CanList { get; set; }
        public bool? CanEdit { get; set; }
        public bool? CanDelete { get; set; }
        public bool? CanActiveDeactive { get; set; }
        public List<MenuItem> Children { get; set; } = new List<MenuItem>();
    }

    //public class MenuPermissionRequest
    //{
    //    public int? GroupId { get; set; }
    //    public int? UserId { get; set; }
    //    public List<int> MenuIds { get; set; }
    //}

    public class PermissionModel
    {
        public int MenuId { get; set; }
        public bool CanAdd { get; set; }
        public bool CanList { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanActiveDeactive { get; set; }
    }

    public class SaveGroupPermissionModel
    {
        public int GroupId { get; set; }
        public int? UserId { get; set; }
        public List<PermissionModel> Permissions { get; set; }
        public string? CreatedBy { get; set; }
    }
}
