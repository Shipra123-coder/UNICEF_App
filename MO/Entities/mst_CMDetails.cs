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
    [Table("mst_CMDetails")]
    public class mst_CMDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CMId { get; set; }

        public Guid? Guid { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(150)]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Designation is required")]
        [MaxLength(200)]
        [Display(Name = "Designation")]
        public string Designation { get; set; }

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
    public class CMDetailsViewModel
    {
        public mst_CMDetails FormModel { get; set; } = new mst_CMDetails();
        public IEnumerable<mst_CMDetails> CMList { get; set; } = new List<mst_CMDetails>();
    }
}
