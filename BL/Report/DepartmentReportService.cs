using DL;
using Microsoft.AspNetCore.Mvc;
using MO.DashBoard;
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
        public async Task<string> GetAgencyWiseReportDataAsync(int agencyId, string activityGuid = null)


        {
            var sb = new StringBuilder();
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@ActivityGuid", (object)activityGuid ?? DBNull.Value),
                    new SqlParameter("@AgencyId", (object)agencyId ?? DBNull.Value),
                    //new SqlParameter("@DepartmentId", (object)deptId ?? DBNull.Value)
                };

                // Executing Procedure via your helper
                var ds = await _iSql.ExecuteProcedure("SP_Manage_Agency_Report", parameters.ToArray());
                //var ds = await _iSql.ExecuteProcedure("SP_Manage_Agency_Report", parameters.ToArray());

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
        #region ALL REPORT
        public async Task<string> ReportAnalize(string departmentId, string agencyId, string sectorId, string activityGuid = null)
        {            
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@ActivityGuid", (object)activityGuid ?? DBNull.Value),
                new SqlParameter("@DepartmentId", (object)(departmentId ?? (object)DBNull.Value)),
                new SqlParameter("@AgencyId", (object)(agencyId ?? (object)DBNull.Value)),
                new SqlParameter("@SectorId", (object)(sectorId ?? (object)DBNull.Value))
            };            
            var ds = await _iSql.ExecuteProcedure("SP_Manage_Department_Report_New", parameters.ToArray());

            var sb = new StringBuilder();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sb.Append(row[0].ToString());
                }
            }
            var jsonResult = sb.ToString();
            return string.IsNullOrEmpty(jsonResult) ? "[]" : jsonResult;
        }
        #endregion

        #region Agency Login Report
        public async Task<string> GetAgencyLoginReport(int agencyId,string? deptId,string? sectorId, string? activityGuid)
        {
            var sb = new StringBuilder();
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@ActivityGuid", (object)activityGuid ?? DBNull.Value),
                    new SqlParameter("@AgencyId", (object)agencyId ?? DBNull.Value),
                    new SqlParameter("@DepartmentId", (object)deptId ?? DBNull.Value),
                    new SqlParameter("@SectorId", (object)sectorId ?? DBNull.Value)
                };                
                var ds = await _iSql.ExecuteProcedure("SP_Manage_AgencyLogin_Report", parameters.ToArray());                               
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
                throw; 
            }
            var result = sb.ToString();
            return string.IsNullOrEmpty(result) ? "[]" : result;
        }
        public async Task<AgencyStatusModel> GetCountByAgencySectorDeptByAgency(int agencyId,string? deptId,string? sectorId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "GetCountByAgencySectorDeptByAgency"),
                new SqlParameter("@AgencyId", agencyId),
                new SqlParameter("@DepartmentId", deptId),
                new SqlParameter("@SectorId", sectorId)
            };

            var ds = await _iSql.ExecuteProcedure(
                "SP_ManageDashboard",
                parameters.ToArray());

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;

            DataRow row = ds.Tables[0].Rows[0];

            return new AgencyStatusModel
            {
                AgencyId = row["AgencyId"] != DBNull.Value
                            ? Convert.ToInt32(row["AgencyId"])
                            : 0,

                AgencyName = row["AgencyName"]?.ToString(),
                AgencyCode = row["AgencyCode"]?.ToString(),

                Description = row["Description"]?.ToString(),

                ActivityCount = row["ActivityCount"] != DBNull.Value
                            ? Convert.ToInt32(row["ActivityCount"])
                            : 0,

                DepartmentCount = row["DepartmentCount"] != DBNull.Value
                            ? Convert.ToInt32(row["DepartmentCount"])
                            : 0,

                SectorCount = row["SectorCount"] != DBNull.Value
                            ? Convert.ToInt32(row["SectorCount"])
                            : 0,

                GoalCount = row["GoalCount"] != DBNull.Value
                            ? Convert.ToInt32(row["GoalCount"])
                            : 0,
                TargetCount = row["TargetCount"] != DBNull.Value
                            ? Convert.ToInt32(row["TargetCount"])
                            : 0,

                PillarCount = row["PillarCount"] != DBNull.Value
                            ? Convert.ToInt32(row["PillarCount"])
                            : 0,
                SubPillarCount = row["SubPillarCount"] != DBNull.Value
                            ? Convert.ToInt32(row["SubPillarCount"])
                            : 0,

                TaskCount = row["TaskCount"] != DBNull.Value
                            ? Convert.ToInt32(row["TaskCount"])
                            : 0,

                BestPracticeCount = row["BestPracticeCount"] != DBNull.Value
                            ? Convert.ToInt32(row["BestPracticeCount"])
                            : 0
            };
        }
        #endregion
        #region Department Login Report
        public async Task<string> GetDepartmentLoginReport(int deptId, string? agencyId, string? sectorId, string? activityGuid)
        {
            var sb = new StringBuilder();
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@ActivityGuid", (object)activityGuid ?? DBNull.Value),
                    new SqlParameter("@AgencyId", (object)agencyId ?? DBNull.Value),
                    new SqlParameter("@DepartmentId", (object)deptId ?? DBNull.Value),
                    new SqlParameter("@SectorId", (object)sectorId ?? DBNull.Value)
                };
                var ds = await _iSql.ExecuteProcedure("SP_Manage_Department_Report_New", parameters.ToArray());
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
                throw;
            }
            var result = sb.ToString();
            return string.IsNullOrEmpty(result) ? "[]" : result;
        }
        public async Task<DepartmentStatusModel> GetCountByAgencySectorDeptByDept(int deptId, string? agencyId, string? sectorId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "GetCountByAgencySectorDeptByDept"),
                new SqlParameter("@AgencyId", agencyId),
                new SqlParameter("@DepartmentId", deptId),
                new SqlParameter("@SectorId", sectorId)
            };

            var ds = await _iSql.ExecuteProcedure(
                "SP_ManageDashboard",
                parameters.ToArray());

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;

            DataRow row = ds.Tables[0].Rows[0];

            return new DepartmentStatusModel
            {
                DepartmentId = row["DepartmentId"] != DBNull.Value
                            ? Convert.ToInt32(row["DepartmentId"])
                            : 0,

                DepartmentName = row["DepartmentName"]?.ToString(),
                //AgencyCode = row["AgencyCode"]?.ToString(),

                Description = row["Description"]?.ToString(),

                ActivityCount = row["ActivityCount"] != DBNull.Value
                            ? Convert.ToInt32(row["ActivityCount"])
                            : 0,

                AgencyCount = row["AgencyCount"] != DBNull.Value
                            ? Convert.ToInt32(row["AgencyCount"])
                            : 0,

                SectorCount = row["SectorCount"] != DBNull.Value
                            ? Convert.ToInt32(row["SectorCount"])
                            : 0,

                GoalCount = row["GoalCount"] != DBNull.Value
                            ? Convert.ToInt32(row["GoalCount"])
                            : 0,
                TargetCount = row["TargetCount"] != DBNull.Value
                            ? Convert.ToInt32(row["TargetCount"])
                            : 0,

                PillarCount = row["PillarCount"] != DBNull.Value
                            ? Convert.ToInt32(row["PillarCount"])
                            : 0,
                SubPillarCount = row["SubPillarCount"] != DBNull.Value
                            ? Convert.ToInt32(row["SubPillarCount"])
                            : 0,

                TaskCount = row["TaskCount"] != DBNull.Value
                            ? Convert.ToInt32(row["TaskCount"])
                            : 0,

                BestPracticeCount = row["BestPracticeCount"] != DBNull.Value
                            ? Convert.ToInt32(row["BestPracticeCount"])
                            : 0
            };
        }
        #endregion
    }
}
