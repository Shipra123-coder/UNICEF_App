using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Entities
{
    [Table("Login_User")]
    public class Login_User
    {
        [Key]
        public long UserId { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string SSOID { get; set; }
        public int? DistrictId { get; set; }
        public int? RoleId { get; set; }
        public int? DepartmentId { get; set; }
        public int? AgencyId { get; set; }
        public int? UserLevel { get; set; }
        public int GroupId { get; set; }
        public string? Designation { get; set; }
        public string? Mobile { get; set; }
        public string? WhatsappMobile { get; set; }
        public string? EmailId { get; set; }
        public int IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
