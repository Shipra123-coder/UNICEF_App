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
    [Table("mst_Pillar")]
    public class mst_Pillar
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PillarId")]
        [Display(Name = "Theme ID")]
        public int PillarId { get; set; }

        [Required(ErrorMessage = "Theme Name is required.")]
        [StringLength(300, ErrorMessage = "Theme Name cannot exceed 300 characters.")]
        [Display(Name = "Theme Name")]
        [Column("PillarName")]
        public string PillarName { get; set; }

        [StringLength(1000, ErrorMessage = "Theme Description cannot exceed 1000 characters.")]
        [Display(Name = "Theme Description")]
        [Column("Discription")]
        public string? Discription { get; set; }

        [Display(Name = "Status")]
        public int? Status { get; set; } // 1 = Active, 0 = Inactive

        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "Theme Image URL")]
        public string? ImageUrl { get; set; }

        [NotMapped]
        [Display(Name = "Theme Image")]
        public IFormFile? ImageFile { get; set; }
    }
    public class ThemeViewModel
    {
        public mst_Pillar FormModel { get; set; } = new mst_Pillar();
        public IEnumerable<mst_Pillar> ThemeList { get; set; } = new List<mst_Pillar>();
    }
}
