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
    [Table("mst_Agency")]
    public class mst_Agency
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AgencyId { get; set; }

        public Guid? Guid { get; set; }

        [Required(ErrorMessage = "Agency Name is required.")]
        [StringLength(300, ErrorMessage = "Agency Name cannot exceed 300 characters.")]
        [Display(Name = "Agency Name")]
        public string AgencyName { get; set; }

        [Required(ErrorMessage = "Agency Code is required.")]
        [StringLength(50, ErrorMessage = "Agency Code cannot exceed 50 characters.")]
        [Display(Name = "Agency Code")]
        public string AgencyCode { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        [StringLength(250, ErrorMessage = "Website link cannot exceed 250 characters.")]
        [Display(Name = "Website Link")]
        public string? Websitelink { get; set; }

        public int? Status { get; set; } // 1 = Active, 0 = Inactive

        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public string? LogoURL { get; set; }

        [NotMapped]
        [Display(Name = "Agency Logo")]
        public IFormFile? LogoFile { get; set; }
    }
    public class AgencyViewModel
    {
        public mst_Agency FormModel { get; set; } = new mst_Agency();
        public IEnumerable<mst_Agency> AgencyList { get; set; } = new List<mst_Agency>();
    }
}
