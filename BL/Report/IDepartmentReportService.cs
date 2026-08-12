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
    }
}
