using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MO.Entities
{
    [Table("mst_NatureOfSupport")]
    public class mst_NatureOfSupport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("NatureSupportId")]
        public long NatureSupportId { get; set; }

        [Required(ErrorMessage = "Nature of Support Name is required.")]
        [StringLength(300, ErrorMessage = "Name cannot exceed 300 characters.")]
        [Display(Name = "Nature of Support Name")]
        [Column("NatureSupportName")]
        public string NatureSupportName { get; set; }

        [Display(Name = "Display Order")]
        [Range(1, int.MaxValue, ErrorMessage = "Display Order must be a positive number.")]
        [Column("DisplayOrder")]
        public int? DisplayOrder { get; set; }

        [Display(Name = "Status")]
        [Column("IsActive")]
        public bool IsActive { get; set; } = true;

        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

    public class NatureOfSupportViewModel
    {
        public mst_NatureOfSupport FormModel { get; set; } = new mst_NatureOfSupport();
        public IEnumerable<mst_NatureOfSupport> NatureOfSupportList { get; set; } = new List<mst_NatureOfSupport>();
    }
}