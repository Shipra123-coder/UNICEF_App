using DL;
using MO.Common;
using MO.Management;
using MO.ProfileUser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.ManageActivity
{
    public class ManageActivity : IManageActivity
    {
        #region Properties
        private readonly ISQLHelper _iSql;
        #endregion
        #region Constructor
        public ManageActivity(ISQLHelper iSql)
        {
            _iSql = iSql;

        }
        #endregion

        #region SaveActivity
        public async Task<result> SaveActivityWithTasks(ActivityMaster model, string createdBy, string agencyId)
        {
            result _result = new result();

            try
            {
                // ======================================
                // DATE PARSING ENGINE
                // ======================================
                DateTime? activityStartDate = null;
                DateTime? activityEndDate = null;

                if (!string.IsNullOrWhiteSpace(model.StartDate))
                {
                    // Parsed using dd-MM-yyyy format
                    activityStartDate = DateTime.ParseExact(model.StartDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }

                if (!string.IsNullOrWhiteSpace(model.EndDate))
                {
                    // Parsed using dd-MM-yyyy format
                    activityEndDate = DateTime.ParseExact(model.EndDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }

                // ======================================
                // STEP 1: UPSERT MAIN ACTIVITY
                // ======================================
                var mainParams = new List<SqlParameter>
        {
            new SqlParameter("@Action", "InsertActivity"), // Action handles both Insert and Update internally inside SP
            new SqlParameter("@ActivityGuid", model.ActivityGuid ?? (object)DBNull.Value),
            new SqlParameter("@ActivityName", model.ActivityName),
            new SqlParameter("@ShortName", model.ShortName),
            new SqlParameter("@Description", model.Description ?? ""),
            new SqlParameter("@UNSector", model.UNSector),
            new SqlParameter("@StartDate", activityStartDate.HasValue ? (object)activityStartDate.Value : DBNull.Value),
            new SqlParameter("@EndDate", activityEndDate.HasValue ? (object)activityEndDate.Value : DBNull.Value),
            new SqlParameter("@HasSubActivity", model.HasSubActivity),
            new SqlParameter("@DeletedSubIds", model.DeletedSubIds ?? ""),
            new SqlParameter("@DeletedTaskIds", model.DeletedTaskIds ?? ""),
            new SqlParameter("@CreatedBy", createdBy),
            new SqlParameter("@AgencyId", agencyId)
        };

                var activityIdResult = await _iSql.ExecuteProcedure("SP_ManageActivity", mainParams.ToArray());

                if (activityIdResult == null || activityIdResult.Tables.Count == 0 || activityIdResult.Tables[0].Rows.Count == 0)
                {
                    throw new Exception("Failed to retrieve Activity identities from database execution response.");
                }

                int activityId = Convert.ToInt32(activityIdResult.Tables[0].Rows[0]["ActivityId"]);
                string activityguid = activityIdResult.Tables[0].Rows[0]["ActivityGuid"].ToString();

                // ======================================
                // CASE 1: WITH SUB-ACTIVITY
                // ======================================
                if (model.HasSubActivity && model.SubActivities != null)
                {
                    foreach (var sub in model.SubActivities)
                    {
                        var subParams = new List<SqlParameter>
                {
                    new SqlParameter("@Action", "UpsertSubActivity"),
                    new SqlParameter("@ActivityId", activityId),
                    new SqlParameter("@SubActivityId", sub.SubActivityId),
                    new SqlParameter("@SubActivityName", sub.SubActivityName ?? ""),
                    new SqlParameter("@CreatedBy", createdBy)
                };

                        var subIdResult = await _iSql.ExecuteProcedureScalarAsync("SP_ManageActivity", subParams.ToArray());
                        int subActivityId = Convert.ToInt32(subIdResult);

                        // Tasks inside the current SubActivity block
                        if (sub.Tasks != null)
                        {
                            foreach (var task in sub.Tasks)
                            {
                                // Skip unpopulated tasks
                                if (string.IsNullOrWhiteSpace(task.TaskDescription))
                                    continue;

                                DateTime? taskStartDate = null;
                                DateTime? taskEndDate = null;

                                if (!string.IsNullOrWhiteSpace(task.StartDate))
                                {
                                    taskStartDate = DateTime.ParseExact(task.StartDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                }
                                if (!string.IsNullOrWhiteSpace(task.EndDate))
                                {
                                    taskEndDate = DateTime.ParseExact(task.EndDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                }

                                var taskParams = new List<SqlParameter>
                        {
                            new SqlParameter("@Action", "UpsertTask"),
                            new SqlParameter("@TaskId", task.TaskId),
                            new SqlParameter("@ActivityId", activityId),
                            new SqlParameter("@SubActivityId", subActivityId),
                            new SqlParameter("@TaskDescription", task.TaskDescription),
                            new SqlParameter("@TaskDetailDescription", task.TaskDetailDescription),
                            new SqlParameter("@TaskStartDate", taskStartDate.HasValue ? (object)taskStartDate.Value : DBNull.Value),
                            new SqlParameter("@TaskEndDate", taskEndDate.HasValue ? (object)taskEndDate.Value : DBNull.Value),
                            new SqlParameter("@CreatedBy", createdBy)
                        };

                                await _iSql.ExecuteProcedureScalarAsync("SP_ManageActivity", taskParams.ToArray());
                            }
                        }
                    }
                }

                // ======================================
                // CASE 2: WITHOUT SUB-ACTIVITY (DIRECT TASKS)
                // ======================================
                else if (!model.HasSubActivity && model.SubActivities != null && model.SubActivities.Count > 0)
                {
                    foreach (var sub in model.SubActivities)
                    {
                        if (sub.Tasks != null && sub.Tasks.Count > 0)
                        {
                            foreach (var task in sub.Tasks)
                            {
                                // Skip unpopulated tasks
                                if (string.IsNullOrWhiteSpace(task.TaskDescription))
                                    continue;

                                DateTime? taskStartDate = null;
                                DateTime? taskEndDate = null;

                                if (!string.IsNullOrWhiteSpace(task.StartDate))
                                {
                                    taskStartDate = DateTime.ParseExact(task.StartDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                }
                                if (!string.IsNullOrWhiteSpace(task.EndDate))
                                {
                                    taskEndDate = DateTime.ParseExact(task.EndDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                }

                                var taskParams = new List<SqlParameter>
                                {
                                    new SqlParameter("@Action", "UpsertTask"),
                                    new SqlParameter("@TaskId", task.TaskId),
                                    new SqlParameter("@ActivityId", activityId),
                                    new SqlParameter("@SubActivityId", 0), // Explicitly assigned 0 indicating no context layout link
                                    new SqlParameter("@TaskDescription", task.TaskDescription ?? ""),
                                    new SqlParameter("@TaskDetailDescription", task.TaskDetailDescription ?? ""),
                                    new SqlParameter("@TaskStartDate", taskStartDate.HasValue ? (object)taskStartDate.Value : DBNull.Value),
                                    new SqlParameter("@TaskEndDate", taskEndDate.HasValue ? (object)taskEndDate.Value : DBNull.Value),
                                    new SqlParameter("@CreatedBy", createdBy)
                                };

                                await _iSql.ExecuteProcedureScalarAsync("SP_ManageActivity", taskParams.ToArray());
                            }
                        }
                    }
                }

                // Setup operational return data wrapper
                _result.status = true;
                _result.message = string.IsNullOrEmpty(model.ActivityGuid)
                    ? "Activity created successfully."
                    : "Activity updated successfully.";
                _result.id = activityguid;
            }
            catch (Exception ex)
            {
                _result.status = false;
                _result.message = "Error: " + ex.Message;
            }

            return _result;
        }

        public async Task<ActivityMaster> GetFullActivityDetails(string? guid)
        {
            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@Action", "GetFullDetails"),
        new SqlParameter("@ActivityGuid", guid ?? (object)DBNull.Value)
    };

            var ds = await _iSql.ExecuteProcedure("SP_ManageActivity", parameters.ToArray());

            // Check if the dataset contains any data rows
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;

            var firstRow = ds.Tables[0].Rows[0];

            // Construct the parent hierarchy model mapping container
            var model = new ActivityMaster
            {
                ActivityGuid = firstRow["ActivityGuid"]?.ToString(),
                ActivityName = firstRow["ActivityName"]?.ToString(),
                ShortName = firstRow["ShortName"]?.ToString(),
                Description = firstRow["Description"]?.ToString(),
                UNSector = firstRow["UNSector"] != DBNull.Value ? Convert.ToInt32(firstRow["UNSector"]) : 0,

                // Convert Activity Dates to "dd-MM-yyyy" string format for UI compatibility
                StartDate = firstRow["ActivityStartDate"] != DBNull.Value ? Convert.ToDateTime(firstRow["ActivityStartDate"]).ToString("dd-MM-yyyy") : null,
                EndDate = firstRow["ActivityEndDate"] != DBNull.Value ? Convert.ToDateTime(firstRow["ActivityEndDate"]).ToString("dd-MM-yyyy") : null,

                HasSubActivity = firstRow["HasSubActivity"] != DBNull.Value && Convert.ToBoolean(firstRow["HasSubActivity"]),
                SubActivities = new List<SubActivityMaster>(),
                Tasks = new List<TaskMaster>() // Assured list initialization for direct tasks context
            };

            // ======================================
            // CASE 1: MAP WITH SUB-ACTIVITIES
            // ======================================
            if (model.HasSubActivity)
            {
                var subGroups = ds.Tables[0].AsEnumerable()
                    .GroupBy(r => r.Field<int?>("SubActivityId"))
                    .Where(g => g.Key != null && g.Key.Value != 0);

                foreach (var subGroup in subGroups)
                {
                    var subModel = new SubActivityMaster
                    {
                        SubActivityId = subGroup.Key.Value,
                        SubActivityName = subGroup.First().Field<string>("SubActivityName"),
                        Tasks = new List<TaskMaster>()
                    };

                    foreach (var row in subGroup)
                    {
                        if (row["TaskId"] != DBNull.Value)
                        {
                            subModel.Tasks.Add(new TaskMaster
                            {
                                TaskId = Convert.ToInt32(row["TaskId"]),
                                TaskDescription = row["TaskDescription"]?.ToString(),
                                TaskDetailDescription = row["TaskDetailDescription"]?.ToString(),

                                // Convert Task dates inside sub-activities to "dd-MM-yyyy" strings
                                StartDate = row["TaskStartDate"] != DBNull.Value ? Convert.ToDateTime(row["TaskStartDate"]).ToString("dd-MM-yyyy") : null,
                                EndDate = row["TaskEndDate"] != DBNull.Value ? Convert.ToDateTime(row["TaskEndDate"]).ToString("dd-MM-yyyy") : null
                            });
                        }
                    }
                    model.SubActivities.Add(subModel);
                }
            }

            // ======================================
            // CASE 2: MAP DIRECT TASKS (NO SUB-ACTIVITY)
            // ======================================
            else
            {
                var taskRows = ds.Tables[0].AsEnumerable()
                    .Where(r => r["TaskId"] != DBNull.Value);

                foreach (var row in taskRows)
                {
                    model.Tasks.Add(new TaskMaster
                    {
                        TaskId = Convert.ToInt32(row["TaskId"]),
                        TaskDescription = row["TaskDescription"]?.ToString(),
                        TaskDetailDescription = row["TaskDetailDescription"]?.ToString(),

                        // Convert standalone main view task dates to "dd-MM-yyyy" strings
                        StartDate = row["TaskStartDate"] != DBNull.Value ? Convert.ToDateTime(row["TaskStartDate"]).ToString("dd-MM-yyyy") : null,
                        EndDate = row["TaskEndDate"] != DBNull.Value ? Convert.ToDateTime(row["TaskEndDate"]).ToString("dd-MM-yyyy") : null
                    });
                }
            }

            return model;
        }
        #endregion

        #region Department Mapping
        public async Task<result> SaveDepartmentMappingAsync(DeptMappingViewModel model, string userId)
        {
            result _result = new result();
            try
            {
                // 1. Purani mapping delete karein (Sync logic using SP Action)
                // Stored Procedure mein Action 'DeleteDeptMapping' handle karein
                var deleteParams = new List<SqlParameter>
                {
                    new SqlParameter("@Action", "DeleteDeptMapping"),
                    new SqlParameter("@ActivityGuid", model.ActivityGuid)
                };
                await _iSql.ExecuteProcedure("SP_ManageActivityDeptMapping", deleteParams.ToArray());

                // 2. Save Nodal Department (IsNodal = 1)
                if (!string.IsNullOrEmpty(model.NodalDepartmentId))
                {
                    var nodalParams = new List<SqlParameter>
                    {
                        new SqlParameter("@Action", "InsertDeptMapping"),
                        new SqlParameter("@ActivityGuid", model.ActivityGuid),
                        new SqlParameter("@DepartmentId", model.NodalDepartmentId),
                        new SqlParameter("@IsNodal", 1), // 1 for Primary/Nodal
                        new SqlParameter("@CreatedBy", userId)
                    };
                    await _iSql.ExecuteProcedure("SP_ManageActivityDeptMapping", nodalParams.ToArray());
                }

                // 3. Save Supporting Departments (IsNodal = 0)
                if (model.SupportingDepartmentIds != null && model.SupportingDepartmentIds.Any())
                {
                    foreach (var deptId in model.SupportingDepartmentIds)
                    {
                        var supportParams = new List<SqlParameter>
                        {
                            new SqlParameter("@Action", "InsertDeptMapping"),
                            new SqlParameter("@ActivityGuid", model.ActivityGuid),
                            new SqlParameter("@DepartmentId", deptId),
                            new SqlParameter("@IsNodal", 0), // 0 for Supporting
                            new SqlParameter("@CreatedBy", userId)
                        };
                        await _iSql.ExecuteProcedure("SP_ManageActivityDeptMapping", supportParams.ToArray());
                    }
                }

                _result.status = true;
                _result.message = "Department mapping updated successfully!";
            }
            catch (Exception ex)
            {
                _result.status = false;
                _result.message = "Error: " + ex.Message;
            }
            return _result;
        }
        public async Task<List<DepartmentMapList>> GetDeptMap(string activityGuid)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@Action", "getList"));
            param.Add(new SqlParameter("@ActivityGuid", activityGuid));

            DataSet ds = await _iSql.ExecuteProcedure("SP_ManageActivityDeptMapping", param.ToArray());

            List<DepartmentMapList> list = new List<DepartmentMapList>();

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(new DepartmentMapList
                    {
                        DepartmentId = Convert.ToInt32(row["DepartmentId"]),
                        DepartmentName = row["DepartmentName"].ToString(),
                        IsNodal = Convert.ToBoolean(row["IsNodal"])
                    });
                }
            }

            return list;
        }
        #endregion
        #region Goal TargetMapping
        public async Task<result> SaveGoalMappingAsync(GoalMappingModel model, string userId)
        {
            result _result = new result();

            try
            {
                // 🔹 1. Purani mapping delete (Activity based)
                var deleteParams = new List<SqlParameter>
        {
            new SqlParameter("@Action", "DeleteGoalMapping"),
            new SqlParameter("@ActivityGuid", model.ActivityGuid)
        };

                await _iSql.ExecuteProcedure("SP_ManageActivityGoalMapping", deleteParams.ToArray());

                // 🔹 2. Insert New Mapping (Goal + Target)
                if (model.MappingData != null && model.MappingData.Any())
                {
                    foreach (var goal in model.MappingData)
                    {
                        foreach (var target in goal.Targets)
                        {
                            var insertParams = new List<SqlParameter>
                    {
                        new SqlParameter("@Action", "InsertGoalMapping"),
                        new SqlParameter("@ActivityGuid", model.ActivityGuid),
                        new SqlParameter("@GoalId", goal.GoalId),
                        new SqlParameter("@TargetId", target.TargetId),
                        //new SqlParameter("@CreatedBy", userId)
                    };

                            await _iSql.ExecuteProcedure("SP_ManageActivityGoalMapping", insertParams.ToArray());
                        }
                    }
                }

                _result.status = true;
                _result.message = "Goal-Target mapping saved successfully!";
            }
            catch (Exception ex)
            {
                _result.status = false;
                _result.message = "Error: " + ex.Message;
            }

            return _result;
        }
        public async Task<List<GoalMapResponse>> GetGoalMappingAsync(string activityGuid)
        {
            var list = new List<GoalMapResponse>();

            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Action", "GetGoalMapping"),
                    new SqlParameter("@ActivityGuid", activityGuid)
                };

                var ds = await _iSql.ExecuteProcedure("SP_ManageActivityGoalMapping", parameters.ToArray());

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(new GoalMapResponse
                    {
                        GoalId = Convert.ToInt32(row["GoalId"]),
                        GoalName = row["GoalName"].ToString(),
                        TargetId = Convert.ToInt32(row["TargetId"]),
                        TargetName = row["TargetName"].ToString()
                    });
                }
            }
            catch (Exception)
            {
                // handle
            }

            return list;
        }
        #endregion

        #region Pillar Sector SubSector Mapping

        public async Task<result> SavePillarMappingAsync(
    PillarMappingModel model,
    string userId)
        {
            result _result = new result();

            try
            {
                // =========================================
                // DELETE OLD MAPPING
                // =========================================

                var deleteParams = new List<SqlParameter>
        {
            new SqlParameter("@Action", "DeletePillarMapping"),

            new SqlParameter("@ActivityGuid",
                model.ActivityGuid)
        };

                await _iSql.ExecuteProcedure(
                    "SP_ManageActivityPillarMapping",
                    deleteParams.ToArray());

                // =========================================
                // INSERT NEW MAPPING
                // THEME + SUBTHEME
                // =========================================

                if (model.MappingData != null
                    && model.MappingData.Any())
                {
                    foreach (var theme in model.MappingData)
                    {
                        if (theme.Sectors != null
                            && theme.Sectors.Any())
                        {
                            foreach (var subTheme in theme.Sectors)
                            {
                                var insertParams =
                                    new List<SqlParameter>
                                {
                            new SqlParameter("@Action",
                                "InsertPillarMapping"),

                            new SqlParameter("@ActivityGuid",
                                model.ActivityGuid),

                            new SqlParameter("@PillarId",
                                theme.PillarId),

                            new SqlParameter("@SectorId",
                                subTheme.SectorId),

                                        // new SqlParameter("@CreatedBy",
                                        //     userId)
                                };

                                await _iSql.ExecuteProcedure(
                                    "SP_ManageActivityPillarMapping",
                                    insertParams.ToArray());
                            }
                        }
                    }
                }

                // =========================================
                // SUCCESS
                // =========================================

                _result.status = true;

                _result.message =
                    "Themes mapping saved successfully!";
            }
            catch (Exception ex)
            {
                _result.status = false;

                _result.message =
                    "Error: " + ex.Message;
            }

            return _result;
        }
        //public async Task<result> SavePillarMappingAsync(PillarMappingModel model, string userId)
        //{
        //    result _result = new result();

        //    try
        //    {
        //        // 🔹 1. Purani mapping delete (Activity based)
        //        var deleteParams = new List<SqlParameter>
        //{
        //    new SqlParameter("@Action", "DeletePillarMapping"),
        //    new SqlParameter("@ActivityGuid", model.ActivityGuid)
        //};

        //        await _iSql.ExecuteProcedure("SP_ManageActivityPillarMapping", deleteParams.ToArray());

        //        // 🔹 2. Insert New Mapping (Pillar + Sector + SubSector)
        //        if (model.MappingData != null && model.MappingData.Any())
        //        {
        //            foreach (var pillar in model.MappingData)
        //            {
        //                foreach (var sector in pillar.Sectors)
        //                {
        //                    foreach (var sub in sector.SubSectors)
        //                    {
        //                        var insertParams = new List<SqlParameter>
        //                {
        //                    new SqlParameter("@Action", "InsertPillarMapping"),
        //                    new SqlParameter("@ActivityGuid", model.ActivityGuid),
        //                    new SqlParameter("@PillarId", pillar.PillarId),
        //                    new SqlParameter("@SectorId", sector.SectorId),
        //                    new SqlParameter("@SubSectorId", sub.SubSectorId),
        //                    //new SqlParameter("@CreatedBy", userId)
        //                };

        //                        await _iSql.ExecuteProcedure("SP_ManageActivityPillarMapping", insertParams.ToArray());
        //                    }
        //                }
        //            }
        //        }

        //        _result.status = true;
        //        _result.message = "Pillar-Sector-SubSector mapping saved successfully!";
        //    }
        //    catch (Exception ex)
        //    {
        //        _result.status = false;
        //        _result.message = "Error: " + ex.Message;
        //    }

        //    return _result;
        //}
        public async Task<List<PillarMapResponse>> GetPillarMappingAsync(string activityGuid)
        {
            var list = new List<PillarMapResponse>();

            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Action", "GetPillarMapping"),
                    new SqlParameter("@ActivityGuid", activityGuid)
                };

                var ds = await _iSql.ExecuteProcedure("SP_ManageActivityPillarMapping", parameters.ToArray());

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(new PillarMapResponse
                    {
                        PillarId = Convert.ToInt32(row["PillarId"]),
                        PillarName = row["PillarName"].ToString(),

                        SectorId = Convert.ToInt32(row["SectorId"]),
                        SectorName = row["SectorName"].ToString(),

                        //SubSectorId = Convert.ToInt32(row["SubSectorId"]),
                        //SubSectorName = row["SubSectorName"].ToString()
                    });
                }
            }
            catch (Exception)
            {
                // handle error if needed
            }

            return list;
        }

        #endregion
        #region ActivityList
        public async Task<List<ActivityMaster>> GetActivityList(string agencyId)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Action", "GetList"));
            param.Add(new SqlParameter("@AgencyId", agencyId));

            DataSet ds = await _iSql.ExecuteProcedure("SP_ManageActivity", param.ToArray());

            List<ActivityMaster> list = new List<ActivityMaster>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new ActivityMaster
                {
                    //ActivityGuid = Convert.ToInt32(row["ActivityId"]),
                    ActivityGuid = row["ActivityGuid"].ToString(),
                    ActivityName = row["ActivityName"].ToString(),
                    ShortName = row["ShortName"].ToString(),
                    Description = row["Description"].ToString(),
                    IsActive = Convert.ToBoolean(row["IsActive"]),
                    ActivityStatus = row["ActivityStatus"].ToString(),
                    CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                    ReportingId = row["ReportingId"].ToString()
                });
            }

            return list;
        }
        #endregion

        #region GeoLevel
        public async Task<result> SaveGeoLevelAsync(GeoLevelModel model, string userId)
        {
            result _result = new result();

            try
            {
                // ==========================================
                // SAVE STATE LEVEL
                // ==========================================

                if (model.GeoLevel == "State")
                {
                    var stateParams = new List<SqlParameter>
            {
                new SqlParameter("@Action", "InsertGeoLevel"),

                new SqlParameter("@TaskId", model.TaskId),

                new SqlParameter("@GeoLevel", "State"),

                new SqlParameter("@DistrictId", DBNull.Value),

                new SqlParameter("@BlockId", DBNull.Value),

                new SqlParameter("@CityId", DBNull.Value),

                new SqlParameter("@CreatedBy", userId)
            };

                    await _iSql.ExecuteProcedure(
                        "SP_ManageActivityGeoLevel",
                        stateParams.ToArray());
                }



                // ==========================================
                // SAVE DISTRICT LEVEL
                // ==========================================

                if (model.GeoLevel == "District"
                    && model.DistrictIds != null
                    && model.DistrictIds.Any())
                {
                    foreach (var districtId in model.DistrictIds)
                    {
                        var districtParams = new List<SqlParameter>
                {
                    new SqlParameter("@Action", "InsertGeoLevel"),

                    new SqlParameter("@TaskId", model.TaskId),

                    new SqlParameter("@GeoLevel", "District"),

                    new SqlParameter("@DistrictId", districtId),

                    new SqlParameter("@BlockId", DBNull.Value),

                    new SqlParameter("@CityId", DBNull.Value),

                    new SqlParameter("@CreatedBy", userId)
                };

                        await _iSql.ExecuteProcedure(
                            "SP_ManageActivityGeoLevel",
                            districtParams.ToArray());
                    }
                }



                // ==========================================
                // SAVE BLOCK LEVEL
                // ==========================================

                if (model.GeoLevel == "Block"
                    && model.Blocks != null
                    && model.Blocks.Any())
                {
                    foreach (var block in model.Blocks)
                    {
                        var blockParams = new List<SqlParameter>
                {
                    new SqlParameter("@Action", "InsertGeoLevel"),

                    new SqlParameter("@TaskId", model.TaskId),

                    new SqlParameter("@GeoLevel", "Block"),

                    new SqlParameter("@DistrictId", block.DistrictId),

                    new SqlParameter("@BlockId", block.BlockId),

                    new SqlParameter("@CityId", DBNull.Value),

                    new SqlParameter("@CreatedBy", userId)
                };

                        await _iSql.ExecuteProcedure(
                            "SP_ManageActivityGeoLevel",
                            blockParams.ToArray());
                    }
                }



                // ==========================================
                // SAVE CITY LEVEL
                // ==========================================

                if (model.GeoLevel == "City"
                    && model.Cities != null
                    && model.Cities.Any())
                {
                    foreach (var city in model.Cities)
                    {
                        var cityParams = new List<SqlParameter>
                {
                    new SqlParameter("@Action", "InsertGeoLevel"),

                    new SqlParameter("@TaskId", model.TaskId),

                    new SqlParameter("@GeoLevel", "City"),

                    new SqlParameter("@DistrictId", city.DistrictId),

                    new SqlParameter("@BlockId", DBNull.Value),

                    new SqlParameter("@CityId", city.CityId),

                    new SqlParameter("@CreatedBy", userId)
                };

                        await _iSql.ExecuteProcedure(
                            "SP_ManageActivityGeoLevel",
                            cityParams.ToArray());
                    }
                }



                _result.status = true;
                _result.message = "Geo Level saved successfully!";
            }
            catch (Exception ex)
            {
                _result.status = false;
                _result.message = "Error : " + ex.Message;
            }

            return _result;
        }
        public async Task<result> DeleteGeoLevelAsync(long geoId)
        {
            result _result = new result();

            try
            {
                var param = new List<SqlParameter>
        {
            new SqlParameter("@Action", "DeleteGeoLevelById"),
            new SqlParameter("@GeoId", geoId)
        };

                await _iSql.ExecuteProcedure("SP_ManageActivityGeoLevel", param.ToArray());

                _result.status = true;
                _result.message = "Deleted Successfully";
            }
            catch (Exception ex)
            {
                _result.status = false;
                _result.message = ex.Message;
            }

            return _result;
        }
        public async Task<string> GetActivityDataAsync(string activityGuid)
        {
            var sb = new StringBuilder();
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@ActivityGuid", activityGuid) };
                var ds = await _iSql.ExecuteProcedure("GetActivityCompleteData", parameters.ToArray());

                // SQL agar bada JSON return karta hai toh wo multiple rows mein split ho sakta hai
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sb.Append(row[0].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sb.ToString();
        }



        public async Task<string> GetActivityDataForReportAsync(string activityGuid,string reportingId)
        {
            var sb = new StringBuilder();
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@ActivityGuid", activityGuid),
                new SqlParameter("@ReportingId", reportingId)};
                var ds = await _iSql.ExecuteProcedure("SP_ManageReporting", parameters.ToArray());

                // SQL agar bada JSON return karta hai toh wo multiple rows mein split ho sakta hai
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sb.Append(row[0].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sb.ToString();
        }

        #endregion


        #region Nature Of Support Mapping
        public async Task<result> SaveNatureOfSupportMappingAsync(NatureOfSupportMappingModel model, string userId)
        {
            result _result = new result();
            try
            {

                var deleteParams = new List<SqlParameter>
        {
            new SqlParameter(
                "@Action",
                "DeleteNatureOfSupportMapping"
            ),

            new SqlParameter(
                "@ActivityGuid",
                model.ActivityGuid
            )
        };

                await _iSql.ExecuteProcedure(
                    "SP_ManageActivityNatureOfSupportMapping",
                    deleteParams.ToArray()
                );


                if (
                    model.SupportData != null
                    &&
                    model.SupportData.Any()
                )
                {
                    foreach (var support in model.SupportData)
                    {
                        foreach (
                            var detail
                            in support.SupportDetails
                        )
                        {
                            var insertParams =
                                new List<SqlParameter>
                            {
                        new SqlParameter(
                            "@Action",
                            "InsertNatureOfSupportMapping"
                        ),

                        new SqlParameter(
                            "@ActivityGuid",
                            model.ActivityGuid
                        ),

                        new SqlParameter(
                            "@SupportId",
                            support.SupportId
                        ),

                        new SqlParameter(
                            "@DetailId",
                            detail.DetailId
                        )

                                    // new SqlParameter(
                                    //     "@CreatedBy",
                                    //     userId
                                    // )
                            };

                            await _iSql.ExecuteProcedure(
                                "SP_ManageActivityNatureOfSupportMapping",
                                insertParams.ToArray()
                            );
                        }
                    }
                }

                _result.status = true;

                _result.message =
                    "Nature Of Support mapping saved successfully!";
            }
            catch (Exception ex)
            {
                _result.status = false;

                _result.message =
                    "Error : " + ex.Message;
            }

            return _result;
        }

        public async Task<List<NatureOfSupportMapResponse>> GetNatureOfSupportMappingAsync(string activityGuid)
        {
            var list = new List<NatureOfSupportMapResponse>();
            try
            {
                var parameters =
                    new List<SqlParameter>
                {
            new SqlParameter(
                "@Action",
                "GetNatureOfSupportMapping"
            ),

            new SqlParameter(
                "@ActivityGuid",
                activityGuid
            )
                };

                var ds =
                    await _iSql.ExecuteProcedure(
                        "SP_ManageActivityNatureOfSupportMapping",
                        parameters.ToArray()
                    );

                foreach (
                    DataRow row
                    in ds.Tables[0].Rows
                )
                {
                    list.Add(
                        new NatureOfSupportMapResponse
                        {
                            SupportId =
                                Convert.ToInt32(
                                    row["NatureSupportId"]
                                ),

                            SupportName =
                                row["NatureSupportName"]
                                .ToString(),

                            DetailId =
                                Convert.ToInt32(
                                    row["SubNatureOfSupportId"]
                                ),

                            DetailName =
                                row["SupportDetailName"]
                                .ToString()
                        }
                    );
                }
            }
            catch (Exception)
            {
                // HANDLE ERROR
            }

            return list;
        }

        #endregion

    }
}
