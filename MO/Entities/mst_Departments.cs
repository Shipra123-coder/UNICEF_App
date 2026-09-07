using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Entities
{
    [Table("mst_Departments")]
    public class mst_Departments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentId { get; set; }

        public Guid? Guid { get; set; }

        [Required(ErrorMessage = "Department Name is required.")]
        [StringLength(200, ErrorMessage = "Department Name cannot exceed 200 characters.")]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }

        [Required(ErrorMessage = "Department Code is required.")]
        [StringLength(50, ErrorMessage = "Department Code cannot exceed 50 characters.")]
        [Display(Name = "Department Code")]
        public string DepartmentCode { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [StringLength(200, ErrorMessage = "HOD name cannot exceed 200 characters.")]
        [Display(Name = "Head of Department")]
        public string? HeadOfDepartment { get; set; }

        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [StringLength(150, ErrorMessage = "Email cannot exceed 150 characters.")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Enter a valid phone number.")]
        [StringLength(100, ErrorMessage = "Phone cannot exceed 100 characters.")]
        public string? Phone { get; set; }

        [StringLength(300, ErrorMessage = "Address cannot exceed 300 characters.")]
        public string? Address { get; set; }

        public int? Status { get; set; } // 1 = Active, 0 = Inactive

        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public string? LogoUrl { get; set; }

        [NotMapped]
        public IFormFile? LogoFile { get; set; }
    }

    public class DepartmentViewModel
    {
        public mst_Departments FormModel { get; set; } = new mst_Departments();
        public IEnumerable<mst_Departments> DepartmentList { get; set; } = new List<mst_Departments>();
    }
}
