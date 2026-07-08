using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.PageAccessRequirement
{
    public class PageAccessRequirement : IAuthorizationRequirement
    {
        public string PageName { get; }

        public PageAccessRequirement(string pageName)
        {
            PageName = pageName;
        }
    }
}
