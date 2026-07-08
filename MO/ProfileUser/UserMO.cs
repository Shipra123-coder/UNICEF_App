using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.ProfileUser
{
    public class UserMO
    {
        public string? Guid { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Name can contain only letters and spaces")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Display Name is required")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Display Name can contain only letters and spaces")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "SSOId is required")]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "SSOId must be alphanumeric only")]
        public string SSOID { get; set; }

        [Required(ErrorMessage = "District is required")]
        public int DistrictId { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public int DepartmentId { get; set; }        

        [Required(ErrorMessage = "UserLevel is required")]        
        public int UserLevel { get; set; }

        [Required(ErrorMessage = "Group is required")]
        public int GroupId { get; set; }

        public int? RoleId { get; set; }

        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Designation is required")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Designation can contain only letters and spaces")]
        public string Designation { get; set; }

        [Required(ErrorMessage = "Mobile is required")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Whatsapp Mobile is required")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter a valid 10-digit WhatsApp number")]
        public string WhatsappMobile { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string EmailId { get; set; }        

        // Dropdown lists
        public IEnumerable<DistrictListDDL> DistrictList { get; set; }
        public IEnumerable<DepartmentListDDL> DepartmentList { get; set; }       
        public IEnumerable<UserLevelListDDL> UserLevelList { get; set; }
        public IEnumerable<GroupDDL> GroupList { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            // STATE Level = Only Department is mandatory
            if (UserLevel == 1) // change this based on your UserLevel value for State
            {
                if (DepartmentId < 0)
                    errors.Add(new ValidationResult("Department is required for State Level user", new[] { "DepartmentId" }));
            }

            // DISTRICT Level = Department + District + Group are mandatory
            if (UserLevel == 2) // change based on your district level ID
            {
                if (DepartmentId < 0)
                    errors.Add(new ValidationResult("Department is required for District Level user", new[] { "DepartmentId" }));

                if (DistrictId <= 0)
                    errors.Add(new ValidationResult("District is required for District Level user", new[] { "DistrictId" }));

                if (GroupId == null || GroupId <= 0)
                    errors.Add(new ValidationResult("Group is required for District Level user", new[] { "GroupId" }));
            }

            return errors;
        }
    }



    public class UserListMO
    {
        public string? Guid { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string SSOID { get; set; }
        public string DistrictName { get; set; }
        public string DepartmentName { get; set; }
        public string RoleName { get; set; }
        public string UserLevel { get; set; }
        public string Designation { get; set; }
        public string Mobile { get; set; }
        public string WhatsappMobile { get; set; }
        public string EmailId { get; set; }
        public bool IsActive { get; set; }
        public string Department { get; set; }
        public string Group { get; set; }
        public string District { get; set; }        

    }

    public class DistrictListDDL
    {
        public int Id { get; set; }
        public string DistrictName { get; set; }
    }

    public class DepartmentListDDL
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
    }
    public class AgencyListDDL
    {
        public int Id { get; set; }
        public string AgencyName { get; set; }
    }
    public class PillarListDDL
    {
        public int Id { get; set; }
        public string PillarName { get; set; }
    }
    public class SectorListDDL
    {
        public int Id { get; set; }
        public string SectorName { get; set; }
    }
    public class SubSectorListDDL
    {
        public int Id { get; set; }
        public string SubSectorName { get; set; }
    }


    public class RoleListDDL
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
    }
    public class UserLevelListDDL
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class GauravDDL
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class GroupDDL
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
