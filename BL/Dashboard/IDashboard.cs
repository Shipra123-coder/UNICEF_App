using MO.DashBoard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Dashboard
{
    public interface IDashboard
    {
        Task<List<ActivityCoverPageModel>> GetCoverPages();
        Task<List<ActivityCoverPageModel>> GetCoverPagesBySector(string sectorId);
        Task<List<SectorVM>> GetSectorWiseCount();
        Task<SectorDashboardModel> GetDashboardCountBySector(int sectorId);
        Task<SectorDashboardModel> GetCountByAgencySectorDept(int? sectorId, int? agencyId, int? departmentId);
        Task<List<AgencyMasterModel>> GetAgencyList();
        Task<AgencyStatusModel> GetDashboardCountByAgency(int agencyId);
        Task<List<ActivityCoverPageModel>> GetCoverPagesByAgency(string agencyId);
        Task<List<DepartmentMasterModel>> GetDepartmentList();
        Task<DepartmentStatusModel> GetDashboardCountByDept(int deptId);


        Task<List<AgencyDepartmentActivityModel>>GetAgencyDepartmentActivityList(int? agencyId);
        Task<List<AgencyDepartmentActivityModel>>GetAgencySectorActivityList(int? agencyId);
        Task<List<SectorDepartmentActivityModel>>GetSectorDepartmentActivityList(int? sectorId);
        Task<List<SectorAgencyActivityModel>>GetSectorAgencyActivityList(int? sectorId);
        Task<List<DepartmentAgencyActivityModel>>GetDepartmentAgencyActivityList(int? departmentId);
        Task<List<DepartmentSectorActivityModel>>GetDepartmentSectorActivityList(int? departmentId);

        #region Chart
        Task<List<AgencyChartModel>> GetAgencyChartData();
        Task<List<DepartmentChartModel>> GetDepartmentChartData();
        Task<GoalChartModal_main> GetGoalChartData();
        Task<List<ActivityStatusChartModel>> GetActivityStatusChartData();
        Task<List<TaskStatusChartModel>> GetTaskStatusChartData();
        #endregion
    }
}
