using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Master
{
    public class GauravMasterMO
    {
        public int? GauravId { get; set; }

        public string? Guid { get; set; }        

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mangal Name is required")]
        [MaxLength(200)]
        public string MangalName { get; set; }

        public bool IsActive { get; set; }        

        public DateTime? CreatedDate { get; set; }
        public long? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }

        // Button name (Save / Update)
        public string? BtnSave { get; set; } = "Save";

        public List<GauravMasterMO>? _list { get; set; }

        public int? StatusId { get; set; }
    }   
}
