using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Master
{
    public class WorkActivityMO
    {
        public int ActivityId { get; set; }

        public string ActivityName { get; set; }

        public string ShortName { get; set; }

        public string ActivityType { get; set; }

        public int Status { get; set; }

        public string Description { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }

    public class WorkActivityDepartmentMappingMO
    {
        public int Id { get; set; }

        public int WorkActivityId { get; set; }

        public int DepartmentId { get; set; }

        public bool IsNodal { get; set; }   // true = Nodal, false = Supportive

        public DateTime CreatedDate { get; set; }

        // Navigation (optional but recommended)
        public WorkActivityMO WorkActivity { get; set; }
        public DepartmentMasterMO Department { get; set; }
    }

    public class WorkActivityViewModel
    {
        // 🔹 Activity Details
        public WorkActivityMO Activity { get; set; } = new WorkActivityMO();

        // 🔹 Department List (for UI)
        public List<DepartmentMasterMO> DepartmentList { get; set; } = new List<DepartmentMasterMO>();

        // 🔹 Selected Departments (Checkbox)
        public List<int> SelectedDepartmentIds { get; set; } = new List<int>();

        // 🔹 Nodal Department (Radio)
        public int? NodalDepartmentId { get; set; }

        // 🔹 Department List (for UI)
        public List<AgencyMasterMO> AgencyList { get; set; } = new List<AgencyMasterMO>();

        // 🔹 Selected Departments (Checkbox)
        public List<int> SelectedAgencyIds { get; set; } = new List<int>();

        // 🔹 Nodal Department (Radio)
        public int? NodalAgencyId { get; set; }


        // 🔹 Existing Mapping (Edit ke liye)
        public List<WorkActivityDepartmentMappingMO> ExistingMappings { get; set; } = new List<WorkActivityDepartmentMappingMO>();
    }
}
