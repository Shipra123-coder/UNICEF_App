using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Entities
{
    [Table("mst_SubNatureOfSupport")]
    public class mst_SubNatureOfSupport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("SubNatureOfSupportId")]
        public long SubNatureOfSupportId { get; set; }

        [Required(ErrorMessage = "Please select Nature of Support.")]
        [Display(Name = "Nature of Support")]
        [Column("NatureSupportId")]
        public long NatureSupportId { get; set; }

        [Required(ErrorMessage = "Support Detail Name is required.")]
        [StringLength(500, ErrorMessage = "Support Detail Name cannot exceed 500 characters.")]
        [Display(Name = "Support Detail Name")]
        [Column("SupportDetailName")]
        public string SupportDetailName { get; set; }

        [Display(Name = "Display Order")]
        [Range(1, int.MaxValue, ErrorMessage = "Display Order must be a positive number.")]
        [Column("DisplayOrder")]
        public int? DisplayOrder { get; set; }

        [Display(Name = "Status")]
        [Column("IsActive")]
        public bool IsActive { get; set; } = true;

        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [ForeignKey("NatureSupportId")]
        public virtual mst_NatureOfSupport? NatureOfSupport { get; set; }
    }

    public class SubNatureOfSupportViewModel
    {
        public mst_SubNatureOfSupport FormModel { get; set; } = new mst_SubNatureOfSupport();
        public IEnumerable<mst_SubNatureOfSupport> SubNatureOfSupportList { get; set; } = new List<mst_SubNatureOfSupport>();
        public IEnumerable<SelectListItem> NatureOfSupportDropdownList { get; set; } = new List<SelectListItem>();
    }
}
