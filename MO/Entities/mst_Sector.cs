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
    [Table("mst_Sector")]
    public class mst_Sector
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SectorId { get; set; }

        [Required(ErrorMessage = "Please select a Pillar.")]
        [Display(Name = "Pillar")]
        public int? PillarId { get; set; } // int? banayein taaki Required validation cleanly trigger ho

        [StringLength(50, ErrorMessage = "Sector Code cannot exceed 50 characters.")]
        [Display(Name = "Sector Code")]
        public string? SectorCode { get; set; }

        [Required(ErrorMessage = "Sector Name is required.")]
        [StringLength(300, ErrorMessage = "Sector Name cannot exceed 300 characters.")]
        [Display(Name = "Sector Name")]
        public string SectorName { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        public int? Status { get; set; } // 1 = Active, 0 = Inactive

        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }       


        [ForeignKey("PillarId")]
        public virtual mst_Pillar? Pillar { get; set; }        
    }
}
