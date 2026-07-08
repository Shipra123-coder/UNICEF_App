using MO.ProfileUser;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Master
{
    public class GauravDistrictMO
    {
        public long? GauravDistId { get; set; }
        public string? Guid { get; set; }

        [Required(ErrorMessage = "District is required")]
        public int? DistrictId { get; set; }
        public List<DistrictListDDL>? DistrictList { get; set; }

        [Required(ErrorMessage = "Gaurav is required")]
        public int? GauraveId { get; set; }
        public List<GauravDDL>? GauravList { get; set; }

        [Required(ErrorMessage = "Item is required")]
        public string GauravName { get; set; }
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Mangal Name is required")]
        public string MangalName { get; set; }
        public string? Vivran { get; set; }

        public string? CreatedBy { get; set; }
        public string? BtnSave { get; set; } = "Save";

        public int? FyId { get; set; }

        public List<GauravDistrictMO>? list { get; set; } = new List<GauravDistrictMO>();
    }
}
