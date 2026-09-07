using Microsoft.AspNetCore.Http;
using MO.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Entities
{
    public class mst_ContactMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContactId { get; set; }

        public Guid? Guid { get; set; }

        [Required(ErrorMessage = "Please select Contact Level")]
        [Display(Name = "Contact Level")]
        public ContactLevelEnum ContactLevel { get; set; }

        [Required(ErrorMessage = "Officer Name is required")]
        [MaxLength(150)]
        [Display(Name = "Officer Name")]
        public string OfficerName { get; set; }

        [Required(ErrorMessage = "Designation is required")]
        [MaxLength(200)]
        [Display(Name = "Designation")]
        public string Designation { get; set; }

        [MaxLength(50)]
        [Display(Name = "Office Landline No.")]
        public string OfficeLandline { get; set; }

        [MaxLength(50)]
        [Display(Name = "Office I.P. Number")]
        public string OfficeIpNumber { get; set; }

        [Required(ErrorMessage = "Email ID is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [MaxLength(150)]
        [Display(Name = "Email ID")]
        public string EmailId { get; set; }

        public string? PhotoUrl { get; set; }

        [NotMapped]
        [Display(Name = "Photo")]
        public IFormFile? PhotoFile { get; set; }

        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; } = 0;

        [Display(Name = "IsActive")]
        public int IsActive { get; set; } = 1; // 1: Active, 0: Inactive

        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class ContactListItemModel
    {
        public int ContactId { get; set; }
        public Guid? Guid { get; set; }
        public int ContactLevel { get; set; }
        public string ContactLevelName { get; set; }
        public string OfficerName { get; set; }
        public string Designation { get; set; }
        public string OfficeLandline { get; set; }
        public string OfficeIpNumber { get; set; }
        public string EmailId { get; set; }
        public string PhotoUrl { get; set; }
        public int DisplayOrder { get; set; }
        public int Status { get; set; }
    }

    public class ContactViewModel
    {
        public mst_ContactMaster FormModel { get; set; } = new mst_ContactMaster();
        public IEnumerable<mst_ContactMaster> ContactList { get; set; } = new List<mst_ContactMaster>();
    }
}
