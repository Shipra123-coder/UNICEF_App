using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Entities
{
    [Table("m_group")]
    public class m_group
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "uniqueidentifier")]
        public Guid? Guid { get; set; }
        [Required(ErrorMessage = "Group Name is required.")]
        [StringLength(150, ErrorMessage = "Group Name cannot exceed 150 characters.")]
        [Display(Name = "Group Name")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Status")]
        public int? IsActive { get; set; } = 1; // 1 = Active, 0 = Inactive

        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
    public class GroupViewModel
    {
        public m_group FormModel { get; set; } = new m_group();
        public IEnumerable<m_group> GroupList { get; set; } = new List<m_group>();
    }
}
