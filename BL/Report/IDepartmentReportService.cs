using Microsoft.AspNetCore.Mvc;
using MO.DashBoard;
using MO.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Report
{
    public interface IDepartmentReportService
    {
        Task<string> GetDepartmentWiseReportDataAsync(int? departmentId, string activityGuid = null);
        Task<string> GetSectorWiseReportDataAsync(int? sectorId, string activityGuid = null);
        Task<string> GetAgencyWiseReportDataAsync(int agencyId, string activityGuid = null);

        #region All Reports Department Login
        Task<string> ReportAnalize(string departmentId,string agencyId,string sectorId, string activityGuid = null);
        #endregion

        #region Agency Login Reports
        Task<string> GetAgencyLoginReport(int agencyId, string? deptId, string? sectorId, string? activityGuid);
        Task<AgencyStatusModel> GetCountByAgencySectorDeptByAgency(int agencyId, string? deptId, string? sectorId);
        #endregion
        #region Department Login Reports
        Task<string> GetDepartmentLoginReport(int deptId, string? agencyId, string? sectorId, string? activityGuid);
        Task<DepartmentStatusModel> GetCountByAgencySectorDeptByDept(int deptId, string? agencyId, string? sectorId);
        #endregion

    }
}
