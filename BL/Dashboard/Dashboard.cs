using DL;
using Microsoft.Extensions.Configuration;
using MO.DashBoard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Dashboard
{
    public class Dashboard : IDashboard
    {
        #region Properties
        private readonly ISQLHelper _iSql;
        #endregion
        #region Constructor
        public Dashboard(ISQLHelper iSql)
        {
            _iSql = iSql;

        }
        #endregion
        public async Task<List<ActivityCoverPageModel>> GetCoverPages()
        {
            var parameters = new List<SqlParameter>
             {
                 new SqlParameter("@Action", "GetCoverPages")
             };

            var ds = await _iSql.ExecuteProcedure("SP_ManageDashboard", parameters.ToArray());

            var list = new List<ActivityCoverPageModel>();
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return list;

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new ActivityCoverPageModel
                {
                    CoverPageId = row["CoverPageId"] != DBNull.Value ? Convert.ToInt64(row["CoverPageId"]) : 0,
                    ActivityGuid = row["ActivityGuid"]?.ToString(),
                    HeadingTitle = row["HeadingTitle"]?.ToString(),
                    DisplayOrder = row["DisplayOrder"] != DBNull.Value ? Convert.ToInt32(row["DisplayOrder"]) : 0,
                    CoverImageUrl = row["CoverImageUrl"]?.ToString(),
                    DescriptionPdfUrl = row["DescriptionPdfUrl"]?.ToString(),
                    DescriptionNotes = row["DescriptionNotes"]?.ToString(),
                    SubActivityId = row["SubActivityId"] != DBNull.Value ? Convert.ToInt64(row["SubActivityId"]) : null,
                    TaskId = row["TaskId"] != DBNull.Value ? Convert.ToInt64(row["TaskId"]) : null,
                    GeoLocationId = row["GeoLocationId"] != DBNull.Value ? Convert.ToInt64(row["GeoLocationId"]) : null,
                    SectorId = row["SectorId"] != DBNull.Value ? Convert.ToInt32(row["SectorId"]) : null,
                    AgencyId = row["AgencyId"] != DBNull.Value ? Convert.ToInt32(row["AgencyId"]) : null,
                    SectorName = row["UNSectorName"]?.ToString(),
                });
            }

            return list.OrderBy(x => x.DisplayOrder).ToList();
        }
        public async Task<List<ActivityCoverPageModel>> GetCoverPagesBySector(string sectorId)
        {
            var parameters = new List<SqlParameter>
             {
                 new SqlParameter("@Action", "GetCoverPagesBySector"),
                 new SqlParameter("@SectorId",sectorId)
             };

            var ds = await _iSql.ExecuteProcedure("SP_ManageDashboard", parameters.ToArray());

            var list = new List<ActivityCoverPageModel>();
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return list;

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new ActivityCoverPageModel
                {
                    CoverPageId = row["CoverPageId"] != DBNull.Value ? Convert.ToInt64(row["CoverPageId"]) : 0,
                    ActivityGuid = row["ActivityGuid"]?.ToString(),
                    HeadingTitle = row["HeadingTitle"]?.ToString(),
                    DisplayOrder = row["DisplayOrder"] != DBNull.Value ? Convert.ToInt32(row["DisplayOrder"]) : 0,
                    CoverImageUrl = row["CoverImageUrl"]?.ToString(),
                    DescriptionPdfUrl = row["DescriptionPdfUrl"]?.ToString(),
                    DescriptionNotes = row["DescriptionNotes"]?.ToString(),
                    SubActivityId = row["SubActivityId"] != DBNull.Value ? Convert.ToInt64(row["SubActivityId"]) : null,
                    TaskId = row["TaskId"] != DBNull.Value ? Convert.ToInt64(row["TaskId"]) : null,
                    GeoLocationId = row["GeoLocationId"] != DBNull.Value ? Convert.ToInt64(row["GeoLocationId"]) : null,
                    SectorId = row["SectorId"] != DBNull.Value ? Convert.ToInt32(row["SectorId"]) : null,
                    AgencyId = row["AgencyId"] != DBNull.Value ? Convert.ToInt32(row["AgencyId"]) : null,
                    SectorName = row["UNSectorName"]?.ToString(),
                });
            }

            return list.OrderBy(x => x.DisplayOrder).ToList();
        }

        public async Task<List<SectorVM>> GetSectorWiseCount()
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "GetSectorWithcount")
            };

            var ds = await _iSql.ExecuteProcedure("SP_ManageDashboard", parameters.ToArray());

            var list = new List<SectorVM>();

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return list;

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new SectorVM
                {
                    SectorId = row["UNSectorId"] != DBNull.Value
                                ? Convert.ToInt32(row["UNSectorId"])
                                : 0,

                    SectorName = row["UNSectorName"]?.ToString(),
                    SectorCode = row["SectorCode"].ToString(),

                    TotalCount = row["TotalCount"] != DBNull.Value
                                ? Convert.ToInt32(row["TotalCount"])
                                : 0
                });
            }

            return list.OrderBy(x => x.SectorName).ToList();
        }
        public async Task<SectorDashboardModel> GetDashboardCountBySector(int sectorId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "GetDashboardCountBySector"),
                new SqlParameter("@SectorId", sectorId)
            };

            var ds = await _iSql.ExecuteProcedure(
                "SP_ManageDashboard",
                parameters.ToArray());

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;

            DataRow row = ds.Tables[0].Rows[0];

            return new SectorDashboardModel
            {
                SectorId = row["UNSectorId"] != DBNull.Value
                            ? Convert.ToInt32(row["UNSectorId"])
                            : 0,

                SectorName = row["UNSectorName"]?.ToString(),
                SectorCode = row["SectorCode"]?.ToString(),

                Description = row["UNDescription"]?.ToString(),

                ActivityCount = row["ActivityCount"] != DBNull.Value
                            ? Convert.ToInt32(row["ActivityCount"])
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
        public async Task<SectorDashboardModel> GetCountByAgencySectorDept(int? sectorId, int? agencyId, int? departmentId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "GetCountByAgencySectorDept"),
                new SqlParameter("@SectorId", sectorId),
                new SqlParameter("@AgencyId", agencyId),
                new SqlParameter("@DepartmentId", departmentId)

            };

            var ds = await _iSql.ExecuteProcedure(
                "SP_ManageDashboard",
                parameters.ToArray());

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;

            DataRow row = ds.Tables[0].Rows[0];

            return new SectorDashboardModel
            {


                ActivityCount = row["ActivityCount"] != DBNull.Value
                            ? Convert.ToInt32(row["ActivityCount"])
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

            };
        }

        #region Agency
        public async Task<List<AgencyMasterModel>> GetAgencyList()
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "GetAgencyList")
            };
            var ds = await _iSql.ExecuteProcedure("SP_ManageDashboard", parameters.ToArray());
            var list = new List<AgencyMasterModel>();
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return list;
            }

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new AgencyMasterModel
                {
                    AgencyId = row["AgencyId"] != DBNull.Value ? Convert.ToInt32(row["AgencyId"]) : 0,
                    Guid = row["Guid"] != DBNull.Value ? Guid.Parse(row["Guid"].ToString()) : Guid.Empty,
                    AgencyName = row["AgencyName"]?.ToString(),
                    AgencyCode = row["AgencyCode"]?.ToString(),
                    Description = row["Description"]?.ToString(),
                    Websitelink = row["Websitelink"]?.ToString(),
                    LogoURL = row["LogoURL"]?.ToString()
                });
            }

            return list;
        }
        public async Task<AgencyStatusModel> GetDashboardCountByAgency(int agencyId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "GetDashboardCountByAgency"),
                new SqlParameter("@AgencyId", agencyId)
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
        public async Task<List<ActivityCoverPageModel>> GetCoverPagesByAgency(string agencyId)
        {
            var parameters = new List<SqlParameter>
             {
                 new SqlParameter("@Action", "GetCoverPagesByAgency"),
                 new SqlParameter("@AgencyId",agencyId)
             };

            var ds = await _iSql.ExecuteProcedure("SP_ManageDashboard", parameters.ToArray());

            var list = new List<ActivityCoverPageModel>();
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return list;

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new ActivityCoverPageModel
                {
                    CoverPageId = row["CoverPageId"] != DBNull.Value ? Convert.ToInt64(row["CoverPageId"]) : 0,
                    ActivityGuid = row["ActivityGuid"]?.ToString(),
                    HeadingTitle = row["HeadingTitle"]?.ToString(),
                    DisplayOrder = row["DisplayOrder"] != DBNull.Value ? Convert.ToInt32(row["DisplayOrder"]) : 0,
                    CoverImageUrl = row["CoverImageUrl"]?.ToString(),
                    DescriptionPdfUrl = row["DescriptionPdfUrl"]?.ToString(),
                    DescriptionNotes = row["DescriptionNotes"]?.ToString(),
                    SubActivityId = row["SubActivityId"] != DBNull.Value ? Convert.ToInt64(row["SubActivityId"]) : null,
                    TaskId = row["TaskId"] != DBNull.Value ? Convert.ToInt64(row["TaskId"]) : null,
                    GeoLocationId = row["GeoLocationId"] != DBNull.Value ? Convert.ToInt64(row["GeoLocationId"]) : null,
                    SectorId = row["SectorId"] != DBNull.Value ? Convert.ToInt32(row["SectorId"]) : null,
                    AgencyId = row["AgencyId"] != DBNull.Value ? Convert.ToInt32(row["AgencyId"]) : null,
                    SectorName = row["UNSectorName"]?.ToString(),
                });
            }

            return list.OrderBy(x => x.DisplayOrder).ToList();
        }
        #endregion
        #region Department 
        public async Task<List<DepartmentMasterModel>> GetDepartmentList()
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "GetDepartmentList")
            };
            var ds = await _iSql.ExecuteProcedure("SP_ManageDashboard", parameters.ToArray());
            var list = new List<DepartmentMasterModel>();
            if (ds == null ||
                ds.Tables.Count == 0 ||
                ds.Tables[0].Rows.Count == 0)
            {
                return list;
            }
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new DepartmentMasterModel
                {
                    DepartmentId = row["DepartmentId"] != DBNull.Value
                                    ? Convert.ToInt32(row["DepartmentId"])
                                    : 0,

                    Guid = row["Guid"] != DBNull.Value
                                    ? Guid.Parse(row["Guid"].ToString())
                                    : Guid.Empty,

                    DepartmentName = row["DepartmentName"]?.ToString(),

                    DepartmentCode = row["DepartmentCode"]?.ToString(),

                    Description = row["Description"]?.ToString(),

                    HeadOfDepartment = row["HeadOfDepartment"]?.ToString(),

                    Email = row["Email"]?.ToString(),

                    Phone = row["Phone"]?.ToString(),

                    Address = row["Address"]?.ToString(),

                });
            }
            return list;
        }
        public async Task<DepartmentStatusModel> GetDashboardCountByDept(int deptId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "GetDashboardCountByDepartment"),
                new SqlParameter("@DepartmentId", deptId)
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
        #region Goal
        public async Task<GoalStatusModel> GetDashboardCountByGoal(int goalId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "GetDashboardCountByGoal"),
                new SqlParameter("@GoalId", goalId)
            };

            var ds = await _iSql.ExecuteProcedure(
                "SP_ManageDashboard",
                parameters.ToArray());

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;

            DataRow row = ds.Tables[0].Rows[0];

            return new GoalStatusModel
            {
                GoalId = row["GoalId"] != DBNull.Value
                            ? Convert.ToInt32(row["GoalId"])
                            : 0,

                GoalName = row["GoalName"]?.ToString(),
                GoalDisplayNumber = Convert.ToInt32(row["DisplayNumber"]),
                Description = row["Tagline"]?.ToString(),
                GoalColor = row["Code"].ToString(),

                DepartmentCount = row["DepartmentCount"] != DBNull.Value
                            ? Convert.ToInt32(row["DepartmentCount"])
                            : 0,
                ActivityCount = row["ActivityCount"] != DBNull.Value
                            ? Convert.ToInt32(row["ActivityCount"])
                            : 0,
                AgencyCount = row["AgencyCount"] != DBNull.Value
                            ? Convert.ToInt32(row["AgencyCount"])
                            : 0,
                SectorCount = row["SectorCount"] != DBNull.Value
                            ? Convert.ToInt32(row["SectorCount"])
                            : 0,
                //GoalCount = row["GoalCount"] != DBNull.Value
                //            ? Convert.ToInt32(row["GoalCount"])
                //            : 0,
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
        public async Task<List<AgencyDepartmentActivityModel>> GetAgencySectorActivityList(int? agencyId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action",
                    "GetAgencySectorActivityList"),

                new SqlParameter("@AgencyId",
                    agencyId ?? (object)DBNull.Value)
            };

            var ds = await _iSql.ExecuteProcedure(
                "SP_ManageActivityList",
                parameters.ToArray());

            var list =
                new List<AgencyDepartmentActivityModel>();

            if (ds == null
                || ds.Tables.Count == 0
                || ds.Tables[0].Rows.Count == 0)
            {
                return list;
            }

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new AgencyDepartmentActivityModel
                {
                    AgencyId =
                        row["AgencyId"] != DBNull.Value
                        ? Convert.ToInt32(row["AgencyId"])
                        : 0,

                    AgencyName =
                        row["AgencyName"]?.ToString(),

                    AgencyCode =
                        row["AgencyCode"]?.ToString(),

                    LogoURL =
                        row["LogoURL"]?.ToString(),

                    UNSectorId =
                        row["UNSectorId"] != DBNull.Value
                        ? Convert.ToInt32(row["UNSectorId"])
                        : null,

                    UNSectorName =
                        row["UNSectorName"]?.ToString(),

                    ActivityId =
                        row["ActivityId"] != DBNull.Value
                        ? Convert.ToInt64(row["ActivityId"])
                        : 0,

                    ActivityName =
                        row["ActivityName"]?.ToString(),

                    //ActivityCode =
                    //    row["ActivityCode"]?.ToString(),

                    ActivityDescription =
                        row["ActivityDescription"]?.ToString(),

                    ActivityStatus =
    row["ActivityStatus"] != DBNull.Value
    ? row["ActivityStatus"].ToString()
    : "",
                    NodalDepartment = row["NodalDepartment"]?.ToString(),
                    AssociatedDepartments = row["AssociatedDepartments"]?.ToString(),
                });
            }

            return list;
        }
        public async Task<List<AgencyDepartmentActivityModel>> GetAgencyDepartmentActivityList(int? agencyId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action",
                    "GetAgencyDepartmentActivityList"),

                new SqlParameter("@AgencyId",
                    agencyId ?? (object)DBNull.Value)
            };

            var ds = await _iSql.ExecuteProcedure(
                "SP_ManageActivityList",
                parameters.ToArray());

            var list =
                new List<AgencyDepartmentActivityModel>();

            if (ds == null
                || ds.Tables.Count == 0
                || ds.Tables[0].Rows.Count == 0)
            {
                return list;
            }

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new AgencyDepartmentActivityModel
                {
                    AgencyId =
                        row["AgencyId"] != DBNull.Value
                        ? Convert.ToInt32(row["AgencyId"])
                        : 0,

                    AgencyName =
                        row["AgencyName"]?.ToString(),

                    AgencyCode =
                        row["AgencyCode"]?.ToString(),

                    LogoURL =
                        row["LogoURL"]?.ToString(),

                    DepartmentId =
                        row["DepartmentId"] != DBNull.Value
                        ? Convert.ToInt32(row["DepartmentId"])
                        : null,

                    NodalDepartment =
                        row["DepartmentName"]?.ToString(),

                    DepartmentCode =
                        row["DepartmentCode"]?.ToString(),

                    ActivityId =
                        row["ActivityId"] != DBNull.Value
                        ? Convert.ToInt64(row["ActivityId"])
                        : 0,

                    ActivityName =
                        row["ActivityName"]?.ToString(),

                    //ActivityCode =
                    //    row["ActivityCode"]?.ToString(),

                    ActivityDescription =
                        row["ActivityDescription"]?.ToString(),

                    ActivityStatus =
                        row["ActivityStatus"]?.ToString(),
                    UNSectorName = row["UNSectorName"]?.ToString(),
                });
            }

            return list;
        }
        public async Task<List<SectorDepartmentActivityModel>> GetSectorDepartmentActivityList(int? sectorId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action",
                    "GetSectorDepartmentActivityList"),

                new SqlParameter("@SectorId",
                    sectorId ?? (object)DBNull.Value)
            };

            var ds = await _iSql.ExecuteProcedure(
                "SP_ManageActivityList",
                parameters.ToArray());

            var list =
                new List<SectorDepartmentActivityModel>();

            if (ds == null
                || ds.Tables.Count == 0
                || ds.Tables[0].Rows.Count == 0)
            {
                return list;
            }

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new SectorDepartmentActivityModel
                {
                    UNSectorId =
                        row["UNSectorId"] != DBNull.Value
                        ? Convert.ToInt32(row["UNSectorId"])
                        : 0,

                    UNSectorName =
                        row["UNSectorName"]?.ToString(),

                    DepartmentId =
                        row["DepartmentId"] != DBNull.Value
                        ? Convert.ToInt32(row["DepartmentId"])
                        : 0,

                    DepartmentName =
                        row["DepartmentName"]?.ToString(),

                    DepartmentCode =
                        row["DepartmentCode"]?.ToString(),

                    ActivityId =
                        row["ActivityId"] != DBNull.Value
                        ? Convert.ToInt64(row["ActivityId"])
                        : 0,

                    ActivityName =
                        row["ActivityName"]?.ToString(),

                    //ActivityCode =
                    //    row["ActivityCode"]?.ToString(),

                    ActivityDescription =
                        row["ActivityDescription"]?.ToString(),

                    AgencyName =
                        row["AgencyName"]?.ToString(),

                    ActivityStatus =
                        row["ActivityStatus"]?.ToString()
                });
            }

            return list;
        }
        public async Task<List<SectorAgencyActivityModel>> GetSectorAgencyActivityList(int? sectorId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action",
                    "GetSectorAgencyActivityList"),

                new SqlParameter("@SectorId",
                    sectorId ?? (object)DBNull.Value)
            };

            var ds = await _iSql.ExecuteProcedure(
                "SP_ManageActivityList",
                parameters.ToArray());

            var list =
                new List<SectorAgencyActivityModel>();

            if (ds == null
                || ds.Tables.Count == 0
                || ds.Tables[0].Rows.Count == 0)
            {
                return list;
            }

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new SectorAgencyActivityModel
                {
                    UNSectorId =
                        row["UNSectorId"] != DBNull.Value
                        ? Convert.ToInt32(row["UNSectorId"])
                        : 0,

                    UNSectorName =
                        row["UNSectorName"]?.ToString(),

                    AgencyId =
                        row["AgencyId"] != DBNull.Value
                        ? Convert.ToInt32(row["AgencyId"])
                        : 0,

                    AgencyName =
                        row["AgencyName"]?.ToString(),

                    AgencyCode =
                        row["AgencyCode"]?.ToString(),

                    LogoURL =
                        row["LogoURL"]?.ToString(),

                    ActivityId =
                        row["ActivityId"] != DBNull.Value
                        ? Convert.ToInt64(row["ActivityId"])
                        : 0,

                    ActivityName =
                        row["ActivityName"]?.ToString(),

                    //ActivityCode =
                    //    row["ActivityCode"]?.ToString(),

                    ActivityDescription =
                        row["ActivityDescription"]?.ToString(),

                    NodalDepartment = row["NodalDepartment"]?.ToString(),
                    AssociatedDepartment = row["AssociatedDepartments"]?.ToString(),

                    IsActive =
                        row["IsActive"]?.ToString(),

                    ActivityStatus =
                        row["ActivityStatus"]?.ToString()
                });
            }

            return list;
        }
        public async Task<List<DepartmentAgencyActivityModel>> GetDepartmentAgencyActivityList(int? departmentId)
        {
            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@Action",
            "GetDepartmentAgencyActivityList"),

        new SqlParameter("@DepartmentId",
            departmentId ?? (object)DBNull.Value)
    };

            var ds = await _iSql.ExecuteProcedure(
                "SP_ManageActivityList",
                parameters.ToArray());

            var list =
                new List<DepartmentAgencyActivityModel>();

            if (ds == null
                || ds.Tables.Count == 0
                || ds.Tables[0].Rows.Count == 0)
            {
                return list;
            }

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new DepartmentAgencyActivityModel
                {
                    DepartmentId =
                        row["DepartmentId"] != DBNull.Value
                        ? Convert.ToInt32(row["DepartmentId"])
                        : 0,

                    DepartmentName =
                        row["DepartmentName"]?.ToString(),
                    AssociatedDepartment = row["AssociatedDepartments"]?.ToString(),

                    UNSectorName = row["UNSectorName"]?.ToString(),

                    AgencyId =
                        row["AgencyId"] != DBNull.Value
                        ? Convert.ToInt32(row["AgencyId"])
                        : 0,

                    AgencyName =
                        row["AgencyName"]?.ToString(),
                    LogoURL = row["LogoURL"] == DBNull.Value ? "" : row["LogoURL"].ToString(),

                    AgencyCode =
                        row["AgencyCode"]?.ToString(),

                    ActivityId =
                        row["ActivityId"] != DBNull.Value
                        ? Convert.ToInt64(row["ActivityId"])
                        : 0,

                    ActivityName =
                        row["ActivityName"]?.ToString(),
                    ActivityStatus =
                        row["ActivityStatus"]?.ToString()
                });
            }

            return list;
        }
        public async Task<List<DepartmentSectorActivityModel>> GetDepartmentSectorActivityList(int? departmentId)
        {
            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@Action",
            "GetDepartmentSectorActivityList"),

        new SqlParameter("@DepartmentId",
            departmentId ?? (object)DBNull.Value)
    };

            var ds = await _iSql.ExecuteProcedure(
                "SP_ManageActivityList",
                parameters.ToArray());

            var list =
                new List<DepartmentSectorActivityModel>();

            if (ds == null
                || ds.Tables.Count == 0
                || ds.Tables[0].Rows.Count == 0)
            {
                return list;
            }

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new DepartmentSectorActivityModel
                {
                    DepartmentId =
                        row["DepartmentId"] != DBNull.Value
                        ? Convert.ToInt32(row["DepartmentId"])
                        : 0,

                    DepartmentName =
                        row["DepartmentName"]?.ToString(),

                    UNSectorId =
                        row["UNSectorId"] != DBNull.Value
                        ? Convert.ToInt32(row["UNSectorId"])
                        : 0,

                    UNSectorName =
                        row["UNSectorName"]?.ToString(),

                    ActivityId =
                        row["ActivityId"] != DBNull.Value
                        ? Convert.ToInt64(row["ActivityId"])
                        : 0,

                    ActivityName =
                        row["ActivityName"]?.ToString(),
                    NodalDepartment = row["NodalDepartment"]?.ToString(),
                    AssociatedDepartments = row["AssociatedDepartments"]?.ToString(),
                    ActivityStatus = row["ActivityStatus"]?.ToString()
                });
            }

            return list;
        }


        #region Chart
        public async Task<List<AgencyChartModel>> GetAgencyChartData()
        {
            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@Action", "GetAgencyChartData")
    };

            var ds = await _iSql.ExecuteProcedure(
                "SP_ManageActivityList",
                parameters.ToArray());

            var list = new List<AgencyChartModel>();

            if (ds == null
                || ds.Tables.Count == 0
                || ds.Tables[0].Rows.Count == 0)
            {
                return list;
            }

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new AgencyChartModel
                {
                    AgencyId =
                        row["AgencyId"] != DBNull.Value
                        ? Convert.ToInt32(row["AgencyId"])
                        : 0,

                    AgencyName =
                        row["AgencyName"]?.ToString(),

                    ActivityCount =
                        row["ActivityCount"] != DBNull.Value
                        ? Convert.ToInt32(row["ActivityCount"])
                        : 0,

                    DepartmentCount =
                        row["DepartmentCount"] != DBNull.Value
                        ? Convert.ToInt32(row["DepartmentCount"])
                        : 0,

                    SectorCount =
                        row["SectorCount"] != DBNull.Value
                        ? Convert.ToInt32(row["SectorCount"])
                        : 0
                });
            }

            return list;
        }
        public async Task<List<DepartmentChartModel>> GetDepartmentChartData()
        {
            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@Action", "GetDepartmentChartData")
    };

            var ds = await _iSql.ExecuteProcedure(
                "SP_ManageActivityList",
                parameters.ToArray());

            var list = new List<DepartmentChartModel>();

            if (ds == null
                || ds.Tables.Count == 0
                || ds.Tables[0].Rows.Count == 0)
            {
                return list;
            }

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new DepartmentChartModel
                {
                    DepartmentId =
                        row["DepartmentId"] != DBNull.Value
                        ? Convert.ToInt32(row["DepartmentId"])
                        : 0,

                    DepartmentName =
                        row["DepartmentName"]?.ToString(),

                    ActivityCount =
                        row["ActivityCount"] != DBNull.Value
                        ? Convert.ToInt32(row["ActivityCount"])
                        : 0,

                    AgencyCount =
                        row["AgencyCount"] != DBNull.Value
                        ? Convert.ToInt32(row["AgencyCount"])
                        : 0,

                    SectorCount =
                        row["SectorCount"] != DBNull.Value
                        ? Convert.ToInt32(row["SectorCount"])
                        : 0
                });
            }

            return list;
        }
        public async Task<GoalChartModal_main> GetGoalChartData()
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "GetGoalChartData")
            };

            var ds = await _iSql.ExecuteProcedure(
                "SP_ManageActivityList",
                parameters.ToArray());

            var _model = new GoalChartModal_main();
            _model.goalchartmodel = new List<GoalChartModel>();

            if (ds == null
                || ds.Tables.Count == 0
                || ds.Tables[0].Rows.Count == 0)
            {
                return _model;
            }

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                _model.goalchartmodel.Add(new GoalChartModel
                {
                    GoalId =
                        row["GoalId"] != DBNull.Value
                        ? Convert.ToInt32(row["GoalId"])
                        : 0,

                    GoalName =
                        row["GoalName"]?.ToString(),
                    GoalImage =
                        row["logo"]?.ToString(),

                    ActivityCount =
                        row["ActivityCount"] != DBNull.Value
                        ? Convert.ToInt32(row["ActivityCount"])
                        : 0,

                    DepartmentCount =
                        row["DepartmentCount"] != DBNull.Value
                        ? Convert.ToInt32(row["DepartmentCount"])
                        : 0,

                    AgencyCount =
                        row["AgencyCount"] != DBNull.Value
                        ? Convert.ToInt32(row["AgencyCount"])
                        : 0,

                    SectorCount =
                        row["SectorCount"] != DBNull.Value
                        ? Convert.ToInt32(row["SectorCount"])
                        : 0
                });
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                _model.activitycount = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalActivity"]);
                _model.departmentcount = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalDepartment"]);
                _model.agencycount = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalAgency"]);
            }
            return _model;
        }
        public async Task<List<ActivityStatusChartModel>> GetActivityStatusChartData()
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "GetActivityStatusChart")
            };

            var ds = await _iSql.ExecuteProcedure(
                "SP_ManageActivityList",
                parameters.ToArray());

            var list = new List<ActivityStatusChartModel>();

            if (ds == null
                || ds.Tables.Count == 0
                || ds.Tables[0].Rows.Count == 0)
            {
                return list;
            }

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new ActivityStatusChartModel
                {
                    ActivityStatus =
                        row["ActivityStatus"]?.ToString(),

                    TotalCount =
                        row["TotalCount"] != DBNull.Value
                        ? Convert.ToInt32(row["TotalCount"])
                        : 0
                });
            }

            return list;
        }

        public async Task<List<TaskStatusChartModel>> GetTaskStatusChartData()
        {
            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@Action", "GetTaskStatusChart")
    };

            var ds = await _iSql.ExecuteProcedure(
                "SP_ManageActivityList",
                parameters.ToArray());

            var list = new List<TaskStatusChartModel>();

            if (ds == null
                || ds.Tables.Count == 0
                || ds.Tables[0].Rows.Count == 0)
            {
                return list;
            }

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new TaskStatusChartModel
                {
                    TaskStatus =
                        row["TaskStatus"]?.ToString(),

                    TotalCount =
                        row["TotalCount"] != DBNull.Value
                        ? Convert.ToInt32(row["TotalCount"])
                        : 0
                });
            }

            return list;
        }
        #endregion

        public async Task<BestPracticesModel> GetCoverPageDetails(long coverPageId)
        {
            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@Action", "GetCoverPageDetails"),
        new SqlParameter("@CoverPageId", coverPageId)
    };

            var ds = await _iSql.ExecuteProcedure("SP_ManageDashboard", parameters.ToArray());

            var model = new BestPracticesModel();

            if (ds == null || ds.Tables.Count == 0)
                return model;

            //==============================
            // Cover Page (Table-0)
            //==============================

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];

                model.CoverPageId = row["CoverPageId"] != DBNull.Value ? Convert.ToInt64(row["CoverPageId"]) : 0;
                model.ActivityGuid = row["ActivityGuid"]?.ToString();
                model.HeadingTitle = row["HeadingTitle"]?.ToString();
                model.DisplayOrder = row["DisplayOrder"] != DBNull.Value ? Convert.ToInt32(row["DisplayOrder"]) : 0;
                model.CoverImageUrl = row["CoverImageUrl"]?.ToString();
                model.DescriptionPdfUrl = row["DescriptionPdfUrl"]?.ToString();
                model.DescriptionNotes = row["DescriptionNotes"]?.ToString();
                model.SubActivityId = row["SubActivityId"] != DBNull.Value ? Convert.ToInt64(row["SubActivityId"]) : null;
                model.TaskId = row["TaskId"] != DBNull.Value ? Convert.ToInt64(row["TaskId"]) : null;
                model.GeoLocationId = row["GeoLocationId"] != DBNull.Value ? Convert.ToInt64(row["GeoLocationId"]) : null;
                model.SectorId = row["SectorId"] != DBNull.Value ? Convert.ToInt32(row["SectorId"]) : null;
                model.AgencyId = row["AgencyId"] != DBNull.Value ? Convert.ToInt32(row["AgencyId"]) : null;
                model.SectorName = row["UNSectorName"]?.ToString();
            }

            //==============================
            // Inner Media (Table-1)
            //==============================

            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    model.MediaList.Add(new BestPracticesInnerMediaModel
                    {
                        InnerMediaId = row["InnerMediaId"] != DBNull.Value ? Convert.ToInt64(row["InnerMediaId"]) : 0,
                        CoverPageId = row["CoverPageId"] != DBNull.Value ? Convert.ToInt64(row["CoverPageId"]) : 0,
                        MediaType = row["MediaType"]?.ToString(),
                        MediaAssetUrl = row["MediaAssetUrl"]?.ToString(),
                        DescriptionNotes = row["DescriptionNotes"]?.ToString()
                    });
                }
            }

            return model;
        }
    }
}
