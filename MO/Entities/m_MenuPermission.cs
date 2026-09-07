using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Entities
{
    public class m_MenuPermission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PermissionId { get; set; }
        public long MenuId { get; set; }
        public int? GroupId { get; set; }
        public int? UserId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? url { get; set; }
        public bool CanAdd { get; set; }
        public bool CanList { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanActiveDeactive { get; set; }

        public virtual m_Menu? Menu { get; set; }
    }
}
