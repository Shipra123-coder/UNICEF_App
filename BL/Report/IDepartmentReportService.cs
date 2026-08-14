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
        Task<string> GetAgencyWiseReportDataAsync(int? sectorId, string activityGuid = null);

    }
}
