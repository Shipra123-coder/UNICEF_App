using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.DashBoard
{
    public class ActivityCoverPageModel
    {
        public long CoverPageId { get; set; }
        public string ActivityGuid { get; set; }
        public string HeadingTitle { get; set; }
        public int DisplayOrder { get; set; }
        public string CoverImageUrl { get; set; }
        public string DescriptionPdfUrl { get; set; }
        public string DescriptionNotes { get; set; }
        public long? SubActivityId { get; set; }
        public long? TaskId { get; set; }
        public long? GeoLocationId { get; set; }
        public int? SectorId { get; set; }
        public int? AgencyId { get; set; }
        public string SectorName { get; set; }     // Optional
        public string LocationName { get; set; }   // Optional
    }

    #region Sector
    public class SectorVM
    {
        public int SectorId { get; set; }
        public string SectorName { get; set; }
        public int TotalCount { get; set; }
    }
    public class SectorDashboardModel
    {
        public int SectorId { get; set; }
        public string SectorName { get; set; }
        public string Description { get; set; }

        public int BestPracticeCount { get; set; }
        public int DepartmentCount { get; set; }
        public int AgencyCount { get; set; }
        public int ActivityCount { get; set; }
        public int TaskCount { get; set; }
        public int GoalCount { get; set; }
        public int TargetCount { get; set; }
        public int PillarCount { get; set; }
        public int SubPillarCount { get; set; }
    }
    #endregion

    #region Agency
    public class AgencyMasterModel
    {
        public int AgencyId { get; set; }
        public Guid Guid { get; set; }
        public string AgencyName { get; set; }
        public string AgencyCode { get; set; }
        public string Description { get; set; }
        public string Websitelink { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string LogoURL { get; set; }
    }
    public class AgencyStatusModel
    {
        public int AgencyId { get; set; }
        public string AgencyName { get; set; }
        public string AgencyCode { get; set; }
        public string Description { get; set; }
        public int BestPracticeCount { get; set; }
        public int DepartmentCount { get; set; }
        public int SectorCount { get; set; }
        public int ActivityCount { get; set; }
        public int TaskCount { get; set; }
        public int GoalCount { get; set; }
        public int TargetCount { get; set; }
        public int PillarCount { get; set; }
        public int SubPillarCount { get; set; }
    }
    #endregion

    #region Department
    public class DepartmentMasterModel
    {
        public int DepartmentId { get; set; }

        public Guid Guid { get; set; }

        public string DepartmentName { get; set; }

        public string DepartmentCode { get; set; }

        public string Description { get; set; }

        public string HeadOfDepartment { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }        
       
    }
    public class DepartmentStatusModel
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public int BestPracticeCount { get; set; }
        public int AgencyCount { get; set; }
        public int SectorCount { get; set; }
        public int ActivityCount { get; set; }
        public int TaskCount { get; set; }
        public int GoalCount { get; set; }
        public int TargetCount { get; set; }
        public int PillarCount { get; set; }
    }
    #endregion
    public class AgencyDepartmentActivityModel
    {
        public int AgencyId { get; set; }

        public string AgencyName { get; set; }

        public string AgencyCode { get; set; }

        public string LogoURL { get; set; }

        public int? DepartmentId { get; set; }

        public string NodalDepartment { get; set; }
        public string AssociatedDepartments { get; set; }

        public string DepartmentCode { get; set; }

        public int? UNSectorId { get; set; }

        public string UNSectorName { get; set; }

        public long ActivityId { get; set; }

        public string ActivityName { get; set; }

        public string ActivityCode { get; set; }

        public string ActivityDescription { get; set; }

        public string ActivityStatus { get; set; }
    }
    public class SectorDepartmentActivityModel
    {
        public int UNSectorId { get; set; }

        public string UNSectorName { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public string DepartmentCode { get; set; }

        public long ActivityId { get; set; }

        public string ActivityName { get; set; }

        public string ActivityCode { get; set; }

        public string ActivityDescription { get; set; }

        public string AgencyName { get; set; }

        public string ActivityStatus { get; set; }
    }
    public class SectorAgencyActivityModel
    {
        public int UNSectorId { get; set; }

        public string UNSectorName { get; set; }

        public int AgencyId { get; set; }

        public string AgencyName { get; set; }

        public string AgencyCode { get; set; }

        public string LogoURL { get; set; }

        public long ActivityId { get; set; }

        public string ActivityName { get; set; }

        public string ActivityCode { get; set; }

        public string ActivityDescription { get; set; }
        public string NodalDepartment { get; set; }
        public string AssociatedDepartment { get; set; }
        public string IsActive { get; set; }
        public string ActivityStatus { get; set; }
    }
    public class DepartmentAgencyActivityModel
    {
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }
        public string AssociatedDepartment { get; set; }

        public string UNSectorName { get; set; }

        public int AgencyId { get; set; }

        public string AgencyName { get; set; }

        public string AgencyCode { get; set; }
        public string LogoURL { get; set; }

        public long ActivityId { get; set; }

        public string ActivityName { get; set; }

        public string ActivityStatus { get; set; }
    }
    public class DepartmentSectorActivityModel
    {
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public int UNSectorId { get; set; }

        public string UNSectorName { get; set; }

        public long ActivityId { get; set; }

        public string ActivityName { get; set; }
        public string NodalDepartment { get; set; }
        public string AssociatedDepartments { get; set; }
        public string ActivityStatus { get; set; }
    }

    public class AgencyChartModel
    {
        public int AgencyId { get; set; }

        public string AgencyName { get; set; }

        public int ActivityCount { get; set; }

        public int DepartmentCount { get; set; }

        public int SectorCount { get; set; }
    }
    public class DepartmentChartModel
    {
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public int ActivityCount { get; set; }

        public int AgencyCount { get; set; }

        public int SectorCount { get; set; }
    }

    public class GoalChartModal_main
    {
        public int activitycount { get; set; }
        public int departmentcount { get; set; }
        public int agencycount { get; set; }
        public List<GoalChartModel> goalchartmodel { get; set; }
    }
    public class GoalChartModel
    {
        public int GoalId { get; set; }

        public string GoalName { get; set; }
        public string GoalImage { get; set; }

        public int ActivityCount { get; set; }

        public int DepartmentCount { get; set; }

        public int AgencyCount { get; set; }

        public int SectorCount { get; set; }
    }
    public class ActivityStatusChartModel
    {
        public string ActivityStatus { get; set; }

        public int TotalCount { get; set; }
    }
    public class TaskStatusChartModel
    {
        public string TaskStatus { get; set; }
        public int TotalCount { get; set; }
    }
}
