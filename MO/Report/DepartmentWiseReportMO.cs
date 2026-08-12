using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Report
{
    // 1. Department-wise Report Response Wrapper
    public class DepartmentMO
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string GeneratedOn { get; set; } = DateTime.Now.ToString("dd-MMM-yyyy");
        public string GeneratedBy { get; set; } = "Administrator";

        // Top Summary Cards Metrics
        public DepartmentSummaryMO Summary { get; set; } = new DepartmentSummaryMO();

        // Department-wise Activities List
        public List<DepartmentActivityDetailMO> Activities { get; set; } = new List<DepartmentActivityDetailMO>();
    }
    // 2. Department Summary DTO (Cards Data)
    public class DepartmentSummaryMO
    {
        public int TotalAgencies { get; set; }
        public int TotalSectors { get; set; }
        public int TotalActivities { get; set; }
        public int TotalTasks { get; set; }
        public int TotalBestPractices { get; set; }
        public int TotalSDGGoals { get; set; }
        public int TotalSDGTargets { get; set; }
        public int TotalVRThemes { get; set; }
        public int TotalVRSubThemes { get; set; }

        // Status Counts
        public int CompletedTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int PendingTasks { get; set; }
    }

    // 3. Activity Detail DTO
    public class DepartmentActivityDetailMO
    {
        public int ActivityId { get; set; }
        public string ActivityGuid { get; set; }
        public string ActivityName { get; set; }
        public string ActivityDescription { get; set; }
        public string ActivityStatus { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }

        // Organization Mapping
        public int SectorId { get; set; }
        public string SectorName { get; set; }

        public int AgencyId { get; set; }
        public string AgencyName { get; set; }

        // Mapped SDG Details
        public string SdgGoal { get; set; }
        public string SdgTarget { get; set; }

        // Mapped Viksit Rajasthan Details
        public string VrTheme { get; set; }
        public string VrSubTheme { get; set; }

        // Child List
        public List<TaskMO> Tasks { get; set; } = new List<TaskMO>();
    }

    //4. Task Detail DTO
    public class TaskMO
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public string Period { get; set; }
        public string StatusCode { get; set; }
        public string AchievementImpact { get; set; }
        public string Remarks { get; set; }
    }
}
