using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Management
{   
    public class mainmodel
    {
        public int ActivityId { get; set; }
        public ActivityMaster activityMaster { get; set; }
        public DeptMappingViewModel deptMappingViewModel { get; set; }
    }
    public class ActivityMaster
    {
        public int? ActivityId { get; set; }
        public string? ActivityGuid { get; set; }   
        [Required]
        public string ActivityName { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public int? UNSector { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public bool HasSubActivity { get; set; }

        public string DeletedSubIds { get; set; }
        public string DeletedTaskIds { get; set; }
        public bool IsActive { get; set; }
        public string? ActivityStatus { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation Property: One Activity -> Many Sub-Activities
        public virtual ICollection<SubActivityMaster> SubActivities { get; set; }
        public List<TaskMaster> Tasks { get; set; } = new List<TaskMaster>(); // 🔥 ADD THIS

        public string ReportingId { get; set; }
    }

    public class SubActivityMaster
    {
        public SubActivityMaster()
        {
            // Initialization zaroori hai
            Tasks = new HashSet<TaskMaster>();
        }

        [Key]
        public int SubActivityId { get; set; }

        public string ActivityGuid { get; set; }
        public string? SubActivityName { get; set; }

        // Navigation Property
        public virtual ICollection<TaskMaster> Tasks { get; set; }
    }
    public class TaskMaster
    {
        [Key]
        public int TaskId { get; set; }
        public int? SubActivityId { get; set; }
        public string TaskDescription { get; set; }
        public string TaskDetailDescription { get; set; }   // New
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }

        [ForeignKey("SubActivityId")]
        public virtual SubActivityMaster? SubActivity { get; set; }
    }

    // Activity aur Departments ke beech Many-to-Many ya Mapping Table
    public class ActivityDepartmentMapping
    {
        [Key]
        public int MappingId { get; set; }

        public string ActivityGuid { get; set; }

        [Required]
        public string DepartmentId { get; set; } // ID like 'Health', 'WCD', etc.

        public bool IsNodal { get; set; } // true = Nodal, false = Supporting

        //[ForeignKey("Guid")]
        //public virtual ActivityMaster Activity { get; set; }
    }

    public class DeptMappingViewModel
    {
        public string ActivityGuid { get; set; }
        public string NodalDepartmentId { get; set; }
        public List<string> SupportingDepartmentIds { get; set; }
    }

    public class DepartmentMapList
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public bool IsNodal { get; set; }
    }

    public class MainModels
    {
        public string? ActivityGuid { get; set; }
        //public int? ActivityId { get; set; }
        public ActivityMaster Activity { get; set; }
        public List<int> DepartmentIds { get; set; }
        public List<int> SectorIds { get; set; }
        public List<int> GoalIds { get; set; }
        public List<int> PillarIds { get; set; }
    }

    #region GoalMapping
    public class GoalMappingModel
    {
        public string ActivityGuid { get; set; }
        public List<GoalMappingData> MappingData { get; set; }
    }

    public class GoalMappingData
    {
        public int GoalId { get; set; }
        public string GoalName { get; set; }
        public List<TargetMappingData> Targets { get; set; }
    }

    public class TargetMappingData
    {
        public int TargetId { get; set; }
        public string TargetName { get; set; }
    }
    public class GoalMapResponse
    {
        public int GoalId { get; set; }
        public string GoalName { get; set; }
        public int TargetId { get; set; }
        public string TargetName { get; set; }
    }
    #endregion

    #region Pillar Mapping
    public class PillarMappingModel
    {
        public string ActivityGuid { get; set; }
        public List<PillarData> MappingData { get; set; }
    }

    public class PillarData
    {
        public int PillarId { get; set; }
        public string PillarName { get; set; }
        public List<SectorData> Sectors { get; set; }
    }

    public class SectorData
    {
        public int SectorId { get; set; }
        public string SectorName { get; set; }
        public List<SubSectorData> SubSectors { get; set; }
    }

    public class SubSectorData
    {
        public int SubSectorId { get; set; }
        public string SubSectorName { get; set; }
    }
    public class PillarMapResponse
    {
        public int PillarId { get; set; }
        public string PillarName { get; set; }

        public int SectorId { get; set; }
        public string SectorName { get; set; }

        public int SubSectorId { get; set; }
        public string SubSectorName { get; set; }
    }
    #endregion

    #region Nature Of Support
    public class NatureOfSupportMappingModel
    {
        public string ActivityGuid { get; set; }

        public List<SupportMappingItem> SupportData { get; set; }
    }
    public class SupportMappingItem
    {
        public int SupportId { get; set; }

        public string SupportName { get; set; }

        public List<SupportDetailItem> SupportDetails { get; set; }
    }
    public class SupportDetailItem
    {
        public int DetailId { get; set; }

        public string DetailName { get; set; }
    }
    public class NatureOfSupportMapResponse
    {
        public int SupportId { get; set; }

        public string SupportName { get; set; }

        public int DetailId { get; set; }

        public string DetailName { get; set; }
    }
    #endregion
    #region ActivityList
    public class ActivityList
    {
        public string ActivityGuid { get; set; }
        public string ActivityName { get; set; }
        public string ShortName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    #endregion
}
