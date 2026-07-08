using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Common
{
    public class MenuPermissionMO
    {
        public int PermissionId { get; set; }
        public int MenuId { get; set; }
        public int GroupId { get; set; }
        public int? UserId { get; set; }
        public bool IsActive { get; set; }
        public string Url { get; set; }

        public bool CanAdd { get; set; }
        public bool CanList { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanActiveDeactive { get; set; }
    }
}
