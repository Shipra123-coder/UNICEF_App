using MO.ProfileUser;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Master
{
    public class GauravDepartmentMO
    {
        public long? Id { get; set; }
        public string? Guid { get; set; }

        [Required(ErrorMessage = "gaurav is required")]
        public int GauravId { get; set; }   // this is nvarchar(500)

        [Required(ErrorMessage = "department is required")]
        public int DepartmentId { get; set; }

        public string? Gaurav { get; set; }   // this is nvarchar(500)
        public string? Department { get; set; }

        public bool IsActive { get; set; } = false;

        public DateTime? CreatedDate { get; set; }
        public long? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }

        // For Dropdown Display
        public List<GauravDDL>? GauravList { get; set; }
        public List<DepartmentListDDL>? DepartmentList { get; set; }      

        // Button name (Save / Update)
        public string? BtnSave { get; set; } = "Save";
        public List<GauravDepartmentMO>? _list { get; set; }
    }
    
}
