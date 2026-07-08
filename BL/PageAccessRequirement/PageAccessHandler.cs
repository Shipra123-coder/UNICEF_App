using DL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.PageAccessRequirement
{
    public class PageAccessHandler : AuthorizationHandler<PageAccessRequirement>
    {
        private readonly ISQLHelper _iSql;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PageAccessHandler(ISQLHelper iSql, IHttpContextAccessor httpContextAccessor)
        {
            _iSql = iSql;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,PageAccessRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var GroupId = httpContext.User.FindFirst("GroupId")?.Value;
            // ✅ Dynamically get controller and action names
            //var endpoint = httpContext.get();
            var routeData = httpContext.GetRouteData();

            string controller = routeData.Values["controller"]?.ToString();
            string action = routeData.Values["action"]?.ToString();

            // ✅ Construct page name as "Controller/Action" or however you store it
            string page = $"/{controller}/{action}";

            if (string.IsNullOrEmpty(GroupId))
                return;

            List<SqlParameter> param = new List<SqlParameter>
             {
                 new SqlParameter("@GroupId",GroupId),
                 new SqlParameter("@PageName", page)
             };

            int count = Convert.ToInt32(await _iSql.ExecuteProcedureScalarAsync("SP_CheckPagePermission", param.ToArray()));

            if (count > 0)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();  // ❗Mark authorization failed
            }
            await Task.CompletedTask;
        }
    }

}
