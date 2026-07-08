using MO.ProfileUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Master
{
    public class UserMasterMO
    {
        public int? UserId { get; set; }

        public Guid Guid { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string DisplayName { get; set; }

        public string SSOID { get; set; }

        public int? DistrictId { get; set; }

        public int? RoleId { get; set; }

        public int? DepartmentId { get; set; }   // Nullable ✔
        public List<DepartmentListDDL> departmentListDDLs { get; set; }

        public int? AgencyId { get; set; }

        public List<AgencyListDDL> agencyListDDLs { get; set; }// Nullable ✔

        public int? UserLevel { get; set; }
        public List<UserLevelListDDL> userLevelListDDLs { get; set; }// Nullable ✔

        public int? GroupId { get; set; }
        public List<GroupDDL> groupListDDLs { get; set; }// Nullable ✔

        public string Designation { get; set; }

        public string Mobile { get; set; }

        public string WhatsappMobile { get; set; }

        public string EmailId { get; set; }

        public int IsActive { get; set; }

       // public DateTime CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        public List<UserMasterMO> list { get; set; }

        //public int? UpdatedBy { get; set; }

        //public DateTime? UpdatedDate { get; set; }


    }
}
