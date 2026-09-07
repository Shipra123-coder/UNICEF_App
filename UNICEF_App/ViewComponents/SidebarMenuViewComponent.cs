using BL.Services.MenuPermission;
using Microsoft.AspNetCore.Mvc;

namespace UNICEF_App.ViewComponents
{
    public class SidebarMenuViewComponent : ViewComponent
    {
        private readonly IMenuPermissionBL _menuService;

        public SidebarMenuViewComponent(IMenuPermissionBL menuService)
        {
            _menuService = menuService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Claims ya Session se logged-in user ka GroupId / RoleId extract karein
            var groupIdClaim = HttpContext.User.FindFirst("GroupId")?.Value;

            // Testing / Default fallback (Agar claim nahi mila toh default 1)
            int groupId = int.TryParse(groupIdClaim, out var gid) ? gid : 1;

            var menuTree = await _menuService.GetAllowedMenusAsync(groupId);

            return View(menuTree);
        }
    }
}
