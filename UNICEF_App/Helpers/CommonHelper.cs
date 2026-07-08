using MO.Common;
using MO.MenuMaster;
using System.Security.Claims;

namespace UNICEF_App.Helpers
{
    public static class CommonHelper
    {
        public static string GetClientIp(HttpContext context)
        {
            string ip = context.Connection.RemoteIpAddress?.ToString();

            // Check Forwarded IP (if behind proxy/load-balancer)
            if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedIps))
            {
                ip = forwardedIps.ToString().Split(',')[0];
            }
            return ip ?? "Unknown IP";
        }
        public static bool IsUserValid(ClaimsPrincipal user, out int groupId, out int userId)
        {
            groupId = 0;
            userId = 0;

            string? groupClaim = user.FindFirstValue("GroupId");
            string? userClaim = user.FindFirstValue("UserId");

            if (!user.Identity.IsAuthenticated ||
                string.IsNullOrEmpty(groupClaim) ||
                string.IsNullOrEmpty(userClaim))
            {
                return false;
            }

            groupId = Convert.ToInt32(groupClaim);
            userId = Convert.ToInt32(userClaim);
            return true;
        }

        public static result HasPermission_AddEdit(string? recordGuid, MenuPermissionMO permission)
        {
            if (recordGuid != null && !permission.CanEdit)
            {
                return new result
                {
                    status = false,
                    message = "You do not have permission to edit this record."
                };
            }

            if (recordGuid == null && !permission.CanAdd)
            {
                return new result
                {
                    status = false,
                    message = "You do not have permission to add a new record."
                };
            }

            // Permission is valid
            return new result { status = true };
        }
        public static result HasPermission_CanActive(string? recordGuid, MenuPermissionMO permission)
        {
            if (recordGuid != null && !permission.CanActiveDeactive)
            {
                return new result
                {
                    status = false,
                    message = "You do not have permission to active and deactive this record."
                };
            }
            // Permission is valid
            return new result { status = true };
        }
        public static result HasPermission_CanDelete(string? recordGuid, MenuPermissionMO permission)
        {
            if (recordGuid != null && !permission.CanDelete)
            {
                return new result
                {
                    status = false,
                    message = "You do not have permission to delete this record."
                };
            }
            // Permission is valid
            return new result { status = true };
        }
    }
}
