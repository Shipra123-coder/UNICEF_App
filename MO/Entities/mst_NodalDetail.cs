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
    [Table("mst_NodalDetail")]
    public class mst_NodalDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NodalId { get; set; }

        public Guid? Guid { get; set; }

        [Required(ErrorMessage = "Officer Name is required")]
        [MaxLength(150)]
        [Display(Name = "Officer Name")]
        public string OfficerName { get; set; }

        [Required(ErrorMessage = "Designation is required")]
        [MaxLength(200)]
        [Display(Name = "Designation")]
        public string Designation { get; set; }

        [Required(ErrorMessage = "Contact Number is required")]
        [MaxLength(50)]
        [RegularExpression(@"^[0-9+\-\s()]{10,15}$", ErrorMessage = "Invalid contact number format")]
        [Display(Name = "Contact No.")]
        public string ContactNo { get; set; }

        [Required(ErrorMessage = "Email ID is required")]
        [MaxLength(150)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email ID")]
        public string EmailId { get; set; }

        [MaxLength(300)]
        public string? PhotoUrl { get; set; }

        [NotMapped]
        [Display(Name = "Photo")]
        public IFormFile? PhotoFile { get; set; }

        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; } = 0;

        [Display(Name = "Status")]
        public int IsActive { get; set; } = 1; // 1: Active, 0: Inactive

        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
    public class NodalDetailViewModel
    {
        public mst_NodalDetail FormModel { get; set; } = new mst_NodalDetail();
        public IEnumerable<mst_NodalDetail> NodalList { get; set; } = new List<mst_NodalDetail>();
    }
}
