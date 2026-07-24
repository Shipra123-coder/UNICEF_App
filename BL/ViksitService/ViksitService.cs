using DL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MO.DashBoard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.ViksitService
{
    public class ViksitService : IViksitService
    {
        #region Properties
        private readonly ISQLHelper _iSql;
        private readonly ILogger<ViksitService> _logger;
        #endregion
        #region Constructor
        public ViksitService(ISQLHelper iSql, ILogger<ViksitService> logger)
        {
            _iSql = iSql;
            _logger = logger;
        }
        #endregion
        public async Task<List<ViksitVM>> GetAllPillarsWithActivityCountAsync()
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "GetAllPillarsWithActivityCount")
            };
            var ds = await _iSql.ExecuteProcedure("SP_ManageViksitRajasthan", parameters.ToArray());
            var list = new List<ViksitVM>();
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return list;

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new ViksitVM
                {
                    ViksitId = row["ViksitId"] != DBNull.Value ? Convert.ToInt32(row["ViksitId"]) : 0,
                    ViksitName = row["ViksitName"]?.ToString(),
                    ImageUrl = row["ImageUrl"] != DBNull.Value ? row["ImageUrl"].ToString() : "",
                    TotalCount = row["TotalCount"] != DBNull.Value ? Convert.ToInt32(row["TotalCount"]) : 0
                });
            }
            return list.OrderBy(x => x.ViksitName).ToList();
        }
        public async Task<ViksitDashboardModel> GetPillarWiseCounts(int viksitId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "GetPillarWiseCounts"),
                new SqlParameter("@ViksitId", viksitId)
            };

            var ds = await _iSql.ExecuteProcedure(
                "SP_ManageViksitRajasthan",
                parameters.ToArray());

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;

            DataRow row = ds.Tables[0].Rows[0];

            return new ViksitDashboardModel
            {
                ViksitId = row["ViksitId"] != DBNull.Value
                            ? Convert.ToInt32(row["ViksitId"])
                            : 0,

                ViksitName = row["ViksitName"]?.ToString(),
                ImageUrl = row["ImageUrl"]?.ToString(),

                Description = row["Discription"]?.ToString(),

                ActivityCount = row["ActivityCount"] != DBNull.Value
                            ? Convert.ToInt32(row["ActivityCount"])
                            : 0,
                SectorCount = row["SectorCount"] != DBNull.Value
                            ? Convert.ToInt32(row["SectorCount"])
                            : 0,
                DepartmentCount = row["DepartmentCount"] != DBNull.Value
                            ? Convert.ToInt32(row["DepartmentCount"])
                            : 0,

                AgencyCount = row["AgencyCount"] != DBNull.Value
                            ? Convert.ToInt32(row["AgencyCount"])
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
        public async Task<List<ViksitDepartmentActivityModel>> GetDepartmentActivityList(int? viksitId)
        {
            var parameters = new List<SqlParameter>
             {
                 new SqlParameter("@Action","GetDepartmentActivityList"),
                 new SqlParameter("@ViksitId",viksitId ?? (object)DBNull.Value)
             };

            var ds = await _iSql.ExecuteProcedure("SP_ManageViksitRajasthan", parameters.ToArray());

            var list = new List<ViksitDepartmentActivityModel>();

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return list;
            }
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new ViksitDepartmentActivityModel
                {
                    ViksitId = row["ViksitId"] != DBNull.Value ? Convert.ToInt32(row["ViksitId"]) : 0,
                    ViksitName = row["ViksitName"]?.ToString(),
                    AssociatedSubThemes = row["AssociatedSubThemes"] == DBNull.Value ? "" : row["AssociatedSubThemes"]?.ToString(),
                    ActivityId = row["ActivityId"] != DBNull.Value ? Convert.ToInt64(row["ActivityId"]) : 0,
                    ActivityName = row["ActivityName"]?.ToString(),
                    ActivityStatus = row["ActivityStatus"]?.ToString(),

                    UNSectorName = row["UNSectorName"]?.ToString(),

                    DepartmentId = row["DepartmentId"] != DBNull.Value ? Convert.ToInt32(row["DepartmentId"]) : 0,
                    DepartmentName = row["DepartmentName"]?.ToString(),

                    NodalDepartmentId = row["NodalDepartmentId"] != DBNull.Value ? Convert.ToInt32(row["NodalDepartmentId"]) : 0,
                    NodalDepartmentName = row["NodalDepartment"]?.ToString(),
                    AssociatedDepartments = row["AssociatedDepartments"]?.ToString(),


                    AgencyName = row["AgencyName"]?.ToString(),
                    LogoURL = row["LogoURL"] == DBNull.Value ? "" : row["LogoURL"].ToString(),

                    AssociatedAgencies = row["AssociatedAgencies"] == DBNull.Value ? "" : row["AssociatedAgencies"].ToString(),
                });
            }
            return list;
        }
        public async Task<List<ViksitAgencyActivityModel>> GetAgencyActivityList(int? viksitId)
        {
            var parameters = new List<SqlParameter>
             {
                 new SqlParameter("@Action","GetAgencyActivityList"),            
                 new SqlParameter("@ViksitId",viksitId ?? (object)DBNull.Value)
             };

            var ds = await _iSql.ExecuteProcedure("SP_ManageViksitRajasthan", parameters.ToArray());

            var list =new List<ViksitAgencyActivityModel>();

            if (ds == null|| ds.Tables.Count == 0|| ds.Tables[0].Rows.Count == 0)
            {
                return list;
            }
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new ViksitAgencyActivityModel
                {
                    ViksitId = row["ViksitId"] != DBNull.Value? Convert.ToInt32(row["ViksitId"]): 0,
                    ViksitName =row["ViksitName"]?.ToString(),

                    ActivityId = row["ActivityId"] != DBNull.Value ? Convert.ToInt64(row["ActivityId"]) : 0,
                    ActivityName = row["ActivityName"]?.ToString(),
                    UNSectorName = row["UNSectorName"]?.ToString(),

                   
                    NodalDepartment = row["NodalDepartment"]?.ToString(),
                    AssociatedDepartments = row["AssociatedDepartments"]?.ToString(),
                    AssociatedSubThemes = row["AssociatedSubThemes"]?.ToString(),


                    AgencyId =row["AgencyId"] != DBNull.Value ? Convert.ToInt32(row["AgencyId"]) : 0,
                    AgencyName =row["AgencyName"]?.ToString(),
                    LogoURL = row["LogoURL"] == DBNull.Value ? "" : row["LogoURL"].ToString(),


                    AgencyCode =row["AgencyCode"]?.ToString(),
                   
                    ActivityStatus = row["ActivityStatus"]?.ToString()
                });
            }
            return list;
        }
        public async Task<List<ViksitSectorActivityModel>> GetSectorActivityList(int? viksitId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action","GetSectorActivityList"),            
                new SqlParameter("@ViksitId",viksitId ?? (object)DBNull.Value)
            };
            var ds = await _iSql.ExecuteProcedure(
                "SP_ManageViksitRajasthan",
                parameters.ToArray());
            var list =new List<ViksitSectorActivityModel>();

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return list;
            }

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new ViksitSectorActivityModel
                {
                    ViksitId = row["ViksitId"] != DBNull.Value ? Convert.ToInt32(row["ViksitId"]) : 0,
                    ViksitName =row["ViksitName"]?.ToString(),
                    UNSectorId =row["UNSectorId"] != DBNull.Value ? Convert.ToInt32(row["UNSectorId"]): 0,
                    UNSectorName =row["UNSectorName"]?.ToString(),
                    ActivityId = row["ActivityId"] != DBNull.Value ? Convert.ToInt64(row["ActivityId"]): 0,
                    ActivityName = row["ActivityName"]?.ToString(),
                    NodalDepartment = row["NodalDepartment"]?.ToString(),
                    AssociatedDepartments = row["AssociatedDepartments"]?.ToString(),
                    AssociatedSubThemes = row["AssociatedSubThemes"]?.ToString(),
                    AgencyName = row["AgencyName"]?.ToString(),
                    ActivityStatus = row["ActivityStatus"]?.ToString()
                });
            }

            return list;
        }
    }
}
