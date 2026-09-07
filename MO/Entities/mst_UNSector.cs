using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MO.Entities
{
    [Table("mst_UNSector")]
    public class mst_UNSector
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UNSectorId { get; set; }

        [Required(ErrorMessage = "Sector Name is required.")]
        [StringLength(300, ErrorMessage = "Sector Name cannot exceed 300 characters.")]
        [Display(Name = "Sector Name")]
        public string UNSectorName { get; set; }

        [Required(ErrorMessage = "Sector Code is required.")]
        [StringLength(50, ErrorMessage = "Sector Code cannot exceed 50 characters.")]
        [Display(Name = "Sector Code")]
        public string UNSectorCode { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        [Display(Name = "Description")]
        public string? UNDescription { get; set; }

        [Display(Name = "Icon URL")]
        public string? UNIconUrl { get; set; }

        [StringLength(20, ErrorMessage = "Color Code cannot exceed 20 characters.")]
        [Display(Name = "Color Code")]
        public string? ColorCode { get; set; } = "#0d6efd";

        [Display(Name = "Status")]
        public int? IsActive { get; set; } = 1; // 1 = Active, 0 = Inactive

        public DateTime? CreatedDate { get; set; }

        [NotMapped]
        [Display(Name = "Sector Icon/Image")]
        public IFormFile? UNIconFile { get; set; }
    }

    public class UNSectorViewModel
    {
        public mst_UNSector FormModel { get; set; } = new mst_UNSector();
        public IEnumerable<mst_UNSector> UNSectorList { get; set; } = new List<mst_UNSector>();
    }

}
