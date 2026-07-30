using DL;
using Microsoft.Extensions.Logging;
using MO.DashBoard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.SDGGoalService
{
    public class SDGGoalService : ISDGGoalServices
    {
        #region Properties
        private readonly ISQLHelper _iSql;
        private readonly ILogger<SDGGoalService> _logger;
        #endregion
        #region Constructor
        public SDGGoalService(ISQLHelper iSql, ILogger<SDGGoalService> logger)
        {
            _iSql = iSql;
            _logger = logger;
        }
        #endregion
        public async Task<List<SDGGoalVM>> GetAllGoalsWithActivityCountAsync()
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "GetAllGoalsWithActivityCount")
            };
            var ds = await _iSql.ExecuteProcedure("SP_ManageSDGGoalProgress", parameters.ToArray());
            var list = new List<SDGGoalVM>();
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return list;

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new SDGGoalVM
                {
                    GoalId = row["GoalId"] != DBNull.Value ? Convert.ToInt32(row["GoalId"]) : 0,
                    GoalName = row["GoalName"]?.ToString(),
                    ImageUrlFront = row["ImageUrlFront"] != DBNull.Value ? row["ImageUrlFront"].ToString() : "",
                    ImageUrlBack = row["ImageUrlBack"] != DBNull.Value ? row["ImageUrlBack"].ToString() : "",
                    DisplayNumber = row["DisplayNumber"] != DBNull.Value ? Convert.ToInt32(row["DisplayNumber"]):0,
                    TotalCount = row["TotalCount"] != DBNull.Value ? Convert.ToInt32(row["TotalCount"]) : 0
                });
            }
            return list.OrderBy(x => x.DisplayNumber).ToList();
        }
        public async Task<SDGGoalDashboardModel> GetGoalWiseCounts(int goalId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "GetGoalsWiseCounts"),
                new SqlParameter("@GoalId", goalId)
            };

            var ds = await _iSql.ExecuteProcedure(
                "SP_ManageSDGGoalProgress",
                parameters.ToArray());

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;

            DataRow row = ds.Tables[0].Rows[0];

            return new SDGGoalDashboardModel
            {
                GoalId = row["GoalId"] != DBNull.Value
                            ? Convert.ToInt32(row["GoalId"])
                            : 0,

                GoalName = row["GoalName"]?.ToString(),
                ImageUrl = row["ImageUrl"]?.ToString(),
                GoalCode = row["GoalColor"].ToString(),

                Description = row["Description"]?.ToString(),

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
        public async Task<List<SDGGoalDepartmentActivityModel>> GetDepartmentActivityList(int? goalId)
        {
            var parameters = new List<SqlParameter>
             {
                 new SqlParameter("@Action","GetDepartmentActivityList"),
                 new SqlParameter("@GoalId",goalId ?? (object)DBNull.Value)
             };

            var ds = await _iSql.ExecuteProcedure("SP_ManageSDGGoalProgress", parameters.ToArray());

            var list = new List<SDGGoalDepartmentActivityModel>();

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return list;
            }
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new SDGGoalDepartmentActivityModel
                {
                    GoalId = row["GoalId"] != DBNull.Value ? Convert.ToInt32(row["GoalId"]) : 0,
                    GoalName = row["GoalName"]?.ToString(),
                    
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

                    AssociatedSubThemes = row["AssociatedSubThemes"] == DBNull.Value ? "" : row["AssociatedSubThemes"]?.ToString(),

                    AssociatedAgencies = row["AssociatedAgencies"] == DBNull.Value ? "" : row["AssociatedAgencies"].ToString(),
                });
            }
            return list;
        }
        public async Task<List<SDGGoalAgencyActivityModel>> GetAgencyActivityList(int? goalId)
        {
            var parameters = new List<SqlParameter>
             {
                 new SqlParameter("@Action","GetAgencyActivityList"),
                 new SqlParameter("@GoalId",goalId ?? (object)DBNull.Value)
             };

            var ds = await _iSql.ExecuteProcedure("SP_ManageSDGGoalProgress", parameters.ToArray());

            var list = new List<SDGGoalAgencyActivityModel>();

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return list;
            }
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new SDGGoalAgencyActivityModel
                {
                    GoalId = row["GoalId"] != DBNull.Value ? Convert.ToInt32(row["GoalId"]) : 0,
                    GoalName = row["GoalName"]?.ToString(),

                    ActivityId = row["ActivityId"] != DBNull.Value ? Convert.ToInt64(row["ActivityId"]) : 0,
                    ActivityName = row["ActivityName"]?.ToString(),
                    UNSectorName = row["UNSectorName"]?.ToString(),


                    NodalDepartment = row["NodalDepartment"]?.ToString(),
                    AssociatedDepartments = row["AssociatedDepartments"]?.ToString(),
                    AssociatedSubThemes = row["AssociatedSubThemes"]?.ToString(),


                    AgencyId = row["AgencyId"] != DBNull.Value ? Convert.ToInt32(row["AgencyId"]) : 0,
                    AgencyName = row["AgencyName"]?.ToString(),
                    LogoURL = row["LogoURL"] == DBNull.Value ? "" : row["LogoURL"].ToString(),


                    AgencyCode = row["AgencyCode"]?.ToString(),

                    ActivityStatus = row["ActivityStatus"]?.ToString()
                });
            }
            return list;
        }
        public async Task<List<SDGGoalSectorActivityModel>> GetSectorActivityList(int? goalId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action","GetSectorActivityList"),
                new SqlParameter("@GoalId",goalId ?? (object)DBNull.Value)
            };
            var ds = await _iSql.ExecuteProcedure(
                "SP_ManageSDGGoalProgress",
                parameters.ToArray());
            var list = new List<SDGGoalSectorActivityModel>();

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return list;
            }

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new SDGGoalSectorActivityModel
                {
                    GoalId = row["GoalId"] != DBNull.Value ? Convert.ToInt32(row["GoalId"]) : 0,
                    GoalName = row["GoalName"]?.ToString(),
                    UNSectorId = row["UNSectorId"] != DBNull.Value ? Convert.ToInt32(row["UNSectorId"]) : 0,
                    UNSectorName = row["UNSectorName"]?.ToString(),
                    ActivityId = row["ActivityId"] != DBNull.Value ? Convert.ToInt64(row["ActivityId"]) : 0,
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
