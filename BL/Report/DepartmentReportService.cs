using DL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Report
{
    public class DepartmentReportService : IDepartmentReportService
    {
        #region Properties
        private readonly ISQLHelper _iSql;
        #endregion
        #region Constructor
        public DepartmentReportService(ISQLHelper iSql)
        {
            _iSql = iSql;

        }
        #endregion

        #region By Department
        public async Task<string> GetDepartmentWiseReportDataAsync(int? departmentId, string activityGuid = null)
        {
            var sb = new StringBuilder();
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@ActivityGuid", (object)activityGuid ?? DBNull.Value),
                    new SqlParameter("@DepartmentId", (object)departmentId ?? DBNull.Value)
                };

                // Executing Procedure via your helper
                var ds = await _iSql.ExecuteProcedure("SP_Manage_Department_Report", parameters.ToArray());

                // SQL agar bada JSON return karta hai toh wo multiple rows mein split ho sakta hai
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        sb.Append(row[0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw; // 'throw ex;' ki jagah 'throw;' use karne se original StackTrace preserve rehta hai
            }

            var result = sb.ToString();
            return string.IsNullOrEmpty(result) ? "[]" : result;
        }
        #endregion
        #region By Sector
        public async Task<string> GetSectorWiseReportDataAsync(int? sectorId, string activityGuid = null)
        {
            var sb = new StringBuilder();
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@ActivityGuid", (object)activityGuid ?? DBNull.Value),
                    new SqlParameter("@SectorId", (object)sectorId ?? DBNull.Value)
                };

                // Executing Procedure via your helper
                var ds = await _iSql.ExecuteProcedure("SP_Manage_Sector_Report", parameters.ToArray());

                // SQL agar bada JSON return karta hai toh wo multiple rows mein split ho sakta hai
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        sb.Append(row[0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw; // 'throw ex;' ki jagah 'throw;' use karne se original StackTrace preserve rehta hai
            }

            var result = sb.ToString();
            return string.IsNullOrEmpty(result) ? "[]" : result;
        }
        #endregion
        #region By Agency
        public async Task<string> GetAgencyWiseReportDataAsync(int? agencyId, string activityGuid = null)
        {
            var sb = new StringBuilder();
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@ActivityGuid", (object)activityGuid ?? DBNull.Value),
                    new SqlParameter("@AgencyId", (object)agencyId ?? DBNull.Value)
                };

                // Executing Procedure via your helper
                var ds = await _iSql.ExecuteProcedure("SP_Manage_Agency_Report", parameters.ToArray());

                // SQL agar bada JSON return karta hai toh wo multiple rows mein split ho sakta hai
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        sb.Append(row[0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw; // 'throw ex;' ki jagah 'throw;' use karne se original StackTrace preserve rehta hai
            }

            var result = sb.ToString();
            return string.IsNullOrEmpty(result) ? "[]" : result;
        }
        #endregion
    }
}
