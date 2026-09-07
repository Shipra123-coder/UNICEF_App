using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MO.ViewModels
{
    public class UserCreateViewModel
    {
        [Required(ErrorMessage = "User name is required")]
        [Display(Name = "User Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must contain at least 1 uppercase, 1 lowercase, 1 number, and 1 special character")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
        public string ConfirmPassword { get; set; }



        [Required(ErrorMessage = "Display Name is required")]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "SSOID is required")]
        [Display(Name = "SSO ID")]
        public string SSOID { get; set; }

        [Required(ErrorMessage = "Please select Group")]
        [Display(Name = "User Group")]
        public int GroupId { get; set; }

        [Required(ErrorMessage = "Please select User Level")]
        [Display(Name = "User Level")]
        public int? UserLevel { get; set; }

        [Display(Name = "Agency")]
        public int? AgencyId { get; set; }

        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }

        [Display(Name = "District")]
        public int? DistrictId { get; set; }

        [Display(Name = "Role")]
        public int? RoleId { get; set; }

        public string Designation { get; set; }

        [Required(ErrorMessage = "Mobile is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Enter valid 10-digit mobile")]
        public string Mobile { get; set; }

        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Enter valid 10-digit WhatsApp mobile")]
        [Display(Name = "WhatsApp Mobile")]
        public string WhatsappMobile { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email ID")]
        public string EmailId { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        public IEnumerable<SelectListItem> GroupList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> UserLevelList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> AgencyList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> DepartmentList { get; set; } = new List<SelectListItem>();
    }

    public class UserEditViewModel : IValidatableObject
    {
        [Required]
        public long Id { get; set; }

        public Guid Guid { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Display Name is required")]
        public string DisplayName { get; set; } = string.Empty;

        [Required(ErrorMessage = "SSOID is required")]
        public string SSOID { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please select a group")]
        public int? GroupId { get; set; }

        [Required(ErrorMessage = "Please select a user level")]
        public int? UserLevel { get; set; }

        public int? AgencyId { get; set; }
        public int? DepartmentId { get; set; }
        public int? DistrictId { get; set; }
        public int? RoleId { get; set; }
        public string? Designation { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Please enter a valid 10-digit mobile number")]
        public string? Mobile { get; set; }

        public string? WhatsappMobile { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string? EmailId { get; set; }

        public bool IsActive { get; set; } = true;

        public IEnumerable<SelectListItem> GroupList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> UserLevelList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> AgencyList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> DepartmentList { get; set; } = new List<SelectListItem>();

        // Custom Cross-Field Validation Logic
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Case 1: Agar Password fill kiya hai
            if (!string.IsNullOrWhiteSpace(Password))
            {
                // Minimum length check (Optional: 6 ya 8 characters)
                if (Password.Length < 6)
                {
                    yield return new ValidationResult(
                        "Password must be at least 6 characters long.",
                        new[] { nameof(Password) }
                    );
                }

                // Confirm Password khali nahi hona chahiye
                if (string.IsNullOrWhiteSpace(ConfirmPassword))
                {
                    yield return new ValidationResult(
                        "Please confirm your new password.",
                        new[] { nameof(ConfirmPassword) }
                    );
                }
                // Dono match hone chahiye
                else if (Password != ConfirmPassword)
                {
                    yield return new ValidationResult(
                        "New password and confirm password do not match.",
                        new[] { nameof(ConfirmPassword) }
                    );
                }
            }
            // Case 2: Agar user ne sirf ConfirmPassword fill kar diya aur Password blank chhod diya
            else if (!string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                yield return new ValidationResult(
                    "Please enter the new password first.",
                    new[] { nameof(Password) }
                );
            }
        }
    }

    public class UserListViewModel
    {
        public long Id { get; set; }

        public Guid Guid { get; set; }

        [Display(Name = "User Name")]
        public string? Name { get; set; }

        [Display(Name = "Display Name")]
        public string? DisplayName { get; set; }

        [Display(Name = "SSO ID")]
        public string? SSOID { get; set; }

        [Display(Name = "Email Address")]
        public string? EmailId { get; set; }

        [Display(Name = "Mobile Number")]
        public string? Mobile { get; set; }

        [Display(Name = "WhatsApp Number")]
        public string? WhatsappMobile { get; set; }

        public string? Designation { get; set; }

        // Foreign Key IDs
        public int? GroupId { get; set; }
        public int? UserLevel { get; set; }
        public int? DepartmentId { get; set; }
        public int? AgencyId { get; set; }
        public int? DistrictId { get; set; }
        public int? RoleId { get; set; }

        // Display Names (Agar Join lagakar text dikhana ho)
        [Display(Name = "Department")]
        public string? DepartmentName { get; set; }

        [Display(Name = "Agency")]
        public string? AgencyName { get; set; }

        [Display(Name = "Role")]
        public string? RoleName { get; set; }

        [Display(Name = "Status")]
        public bool IsActive { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }
    }
}
