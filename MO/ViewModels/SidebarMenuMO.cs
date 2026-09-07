using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.ViewModels
{
    public class SidebarMenuDto
    {
        public long MenuId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? MangalName { get; set; }
        public string? Controller { get; set; }
        public string? ActionName { get; set; }
        public string? Icon { get; set; }
        public bool IsSubMenu { get; set; }
        public int? MainMenuId { get; set; }
        public int? OrderNumber { get; set; }

        // Active Sub-Menus list
        public List<SidebarMenuDto> SubMenus { get; set; } = new();
    }
}
