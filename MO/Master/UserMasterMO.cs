using Microsoft.AspNetCore.Mvc.Rendering;
using MO.ProfileUser;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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



    public class UserCreateViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "User Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Display Name is required")]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "SSOID is required")]
        [Display(Name = "SSO ID")]
        public string SSOID { get; set; }

        [Required(ErrorMessage = "Please select User Group")]
        [Display(Name = "User Group")]
        public int GroupId { get; set; }

        [Required(ErrorMessage = "Please select User Level")]
        [Display(Name = "User Level")]
        public int? UserLevel { get; set; }

        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }

        [Display(Name = "Agency")]
        public int? AgencyId { get; set; }

        [Display(Name = "District")]
        public int? DistrictId { get; set; }

        [Display(Name = "Role")]
        public int? RoleId { get; set; }

        public string Designation { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        [Phone]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        public string Mobile { get; set; }

        [Phone]
        [Display(Name = "WhatsApp Mobile")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Enter a valid 10-digit WhatsApp number")]
        public string WhatsappMobile { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email ID")]
        public string EmailId { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        // Dropdown Data Sources
        public IEnumerable<SelectListItem> GroupList { get; set; }
        public IEnumerable<SelectListItem> UserLevelList { get; set; }
        public IEnumerable<SelectListItem> AgencyList { get; set; }
    }
}
