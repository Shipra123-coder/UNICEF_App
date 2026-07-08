using DL;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MO.Common;
using MO.Master;
using MO.ProfileUser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Common
{
    public class Common : ICommon
    {
        #region Properties
        private readonly ISQLHelper _iSql;
        #endregion
        #region Constructor
        public Common(ISQLHelper iSql)
        {
            _iSql = iSql;

        }
        #endregion
        #region BindGauravDropDown
        public async Task<result> BindGauravDropDown(string userId)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Action", "BindGauravDropDown"));
                param.Add(new SqlParameter("@Id", userId));
                DataSet ds = await _iSql.ExecuteProcedure("SP_GetData", param.ToArray());
                if (ds.Tables.Count > 0)
                {
                    return new result
                    {
                        status = true,
                        data = ds.Tables[0].AsEnumerable().Select(x => new
                        {
                            id = Convert.ToInt32(x["Id"]),
                            name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Convert.ToString(x["Name"]).ToLower()),
                        }).ToList()
                    };
                }
                else
                {
                    return new result
                    {
                        status = false,
                        message = "Something went wrong",
                    };
                }
            }
            catch (Exception ex)
            {
                return new result
                {
                    status = false,
                    message = "Something went wrong",
                };
            }

        }
        #endregion
        #region BindDistrictDropDown
        public async Task<result> BindDistrictDropDown(string userId)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Action", "BindDistrictDropDown"));
                param.Add(new SqlParameter("@Id", userId));
                DataSet ds = await _iSql.ExecuteProcedure("SP_GetData", param.ToArray());
                if (ds.Tables.Count > 0)
                {
                    return new result
                    {
                        status = true,
                        data = ds.Tables[0].AsEnumerable().Select(x => new
                        {
                            Id = Convert.ToInt32(x["Id"]),
                            Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Convert.ToString(x["Name"]).ToLower()),
                        }).ToList()
                    };
                }
                else
                {
                    return new result
                    {
                        status = false,
                        message = "Something went wrong",
                    };
                }
            }
            catch (Exception ex)
            {
                return new result
                {
                    status = false,
                    message = "Something went wrong",
                };
            }

        }
        #endregion

        #region Dropdown
        //public async Task<List<DistrictListDDL>> DDL_DistrictAsync()
        //    {
        //    var list = new List<DistrictListDDL>();
        //    var ds = await _iSql.ExecuteProcedure("SP_GetData", new SqlParameter("@Action", "BindDistrictDropDownAll"));

        //    if (ds.Tables.Count > 0)
        //    {
        //        foreach (DataRow row in ds.Tables[0].Rows)
        //        {
        //            list.Add(new DistrictListDDL
        //            {
        //                Id = Convert.ToInt32(row["Id"]),
        //                DistrictName = row["Name"].ToString()
        //            });
        //        }
        //    }
        //    return list;
        //}

        public async Task<List<DistrictListDDL>> DDL_DistrictByUserIdAsync(string userId)
        {
            var list = new List<DistrictListDDL>();
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Action", "BindDistrictDropDown"));
            param.Add(new SqlParameter("@Id", userId));
            DataSet ds = await _iSql.ExecuteProcedure("SP_GetData", param.ToArray());
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(new DistrictListDDL
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        DistrictName = row["Name"].ToString()
                    });
                }
            }
            return list;
        }

        public async Task<List<RoleListDDL>> DDL_RoleAsync()
        {
            var list = new List<RoleListDDL>();
            
            var ds = await _iSql.ExecuteProcedure("SP_GetData", new SqlParameter("@Action", "DDL_Role"));
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(new RoleListDDL
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        RoleName = row["RoleName"].ToString()
                    });
                }
            }
            return list;
        }

       
        public async Task<List<UserLevelListDDL>> DDL_UserLevelAsync()
        {
            var list = new List<UserLevelListDDL>();

            var ds = await _iSql.ExecuteProcedure("SP_GetData", new SqlParameter("@Action", "DDL_UserLevel"));
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(new UserLevelListDDL
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Name = row["Name"].ToString()
                    });
                }
            }
            return list;
        }

        

        public async Task<List<GroupDDL>> DDL_GroupAsync()
        {
            var list = new List<GroupDDL>();

            var ds = await _iSql.ExecuteProcedure("SP_GetData", new SqlParameter("@Action", "BindDropDownGroup"));
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(new GroupDDL
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Name = row["Name"].ToString()
                    });
                }
            }
            return list;
        }

        public async Task<List<FinancialYearDDL>> DDL_FinancialYearAsync()
        {
            var list = new List<FinancialYearDDL>();

            var ds = await _iSql.ExecuteProcedure("SP_GetData", new SqlParameter("@Action", "BindDropDownFinancialYear"));
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(new FinancialYearDDL
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Name = row["Name"].ToString()
                    });
                }
            }
            return list;
        }

        public async Task<List<FinancialYearListMO>> FinancialYearList(FinancialYearListMO modal)
        {
            List<FinancialYearListMO> list = new List<FinancialYearListMO>();

            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Action", "GetList"));
                param.Add(new SqlParameter("@Id", modal.FinancialYear_Id));

                DataSet ds = await _iSql.ExecuteProcedure("SP_MaganeFinancialYear", param.ToArray());

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        list.Add(new FinancialYearListMO
                        {
                            FinancialYear_Id = row["FinancialYear_Id"] != DBNull.Value ? Convert.ToInt32(row["FinancialYear_Id"]) : null,
                            Guid = row["Guid"]?.ToString(),
                            Code = row["Code"]?.ToString(),
                            Year = row["Year"]?.ToString(),
                            FinancialYear = row["FinancialYear"]?.ToString(),
                            Status = row["Status"] != DBNull.Value && Convert.ToBoolean(row["Status"]),
                            IsCurrent = row["IsCurrent"] != DBNull.Value && Convert.ToBoolean(row["IsCurrent"]),
                            CreatedOn = row["CreatedOn"] == DBNull.Value ? null : Convert.ToDateTime(row["CreatedOn"]),                                                      
                        });
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                return list;
            }
        }
        #endregion

        public async Task<result> DDL_AgencyAsync()
        {

            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Action", "DDL_Agency"));               
                DataSet ds = await _iSql.ExecuteProcedure("SP_GetData", param.ToArray());
                if (ds.Tables.Count > 0)
                {
                    return new result
                    {
                        status = true,
                        data = ds.Tables[0].AsEnumerable().Select(x => new
                        {
                            id = Convert.ToInt32(x["Id"]),
                            name = Convert.ToString(x["AgencyName"]),
                        }).ToList()
                    };
                }
                else
                {
                    return new result
                    {
                        status = false,
                        message = "Something went wrong",
                    };
                }
            }
            catch (Exception ex)
            {
                return new result
                {
                    status = false,
                    message = "Something went wrong",
                };
            }
        }       
        public async Task<result> DDL_DepartmentAsync()
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Action", "DDL_Department"));
                DataSet ds = await _iSql.ExecuteProcedure("SP_GetData", param.ToArray());
                if (ds.Tables.Count > 0)
                {
                    return new result
                    {
                        status = true,
                        data = ds.Tables[0].AsEnumerable().Select(x => new
                        {
                            id = Convert.ToInt32(x["Id"]),
                            name = Convert.ToString(x["DepartmentName"]),
                        }).ToList()
                    };
                }
                else
                {
                    return new result
                    {
                        status = false,
                        message = "Something went wrong",
                    };
                }
            }
            catch (Exception ex)
            {
                return new result
                {
                    status = false,
                    message = "Something went wrong",
                };
            }

        }
        public async Task<result> DDL_SupDepartmentAsync(int? Id)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Action", "DDL_SupDepartment"));
                param.Add(new SqlParameter("@Id", Id));
                DataSet ds = await _iSql.ExecuteProcedure("SP_GetData", param.ToArray());
                if (ds.Tables.Count > 0)
                {
                    return new result
                    {
                        status = true,
                        data = ds.Tables[0].AsEnumerable().Select(x => new
                        {
                            id = Convert.ToInt32(x["Id"]),
                            name = Convert.ToString(x["DepartmentName"]),
                        }).ToList()
                    };
                }
                else
                {
                    return new result
                    {
                        status = false,
                        message = "Something went wrong",
                    };
                }
            }
            catch (Exception ex)
            {
                return new result
                {
                    status = false,
                    message = "Something went wrong",
                };
            }

        }
        public async Task<result> DDL_PillerAsync()
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Action", "DDL_Pillar"));
                DataSet ds = await _iSql.ExecuteProcedure("SP_GetData", param.ToArray());
                if (ds.Tables.Count > 0)
                {
                    return new result
                    {
                        status = true,
                        data = ds.Tables[0].AsEnumerable().Select(x => new
                        {
                            id = Convert.ToInt32(x["Id"]),
                            name = Convert.ToString(x["PillarName"]),
                        }).ToList()
                    };
                }
                else
                {
                    return new result
                    {
                        status = false,
                        message = "Something went wrong",
                    };
                }
            }
            catch (Exception ex)
            {
                return new result
                {
                    status = false,
                    message = "Something went wrong",
                };
            }

        }
        public async Task<result> DDL_SectorAsync(int? Id)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Action", "DDL_Sector"));
                param.Add(new SqlParameter("@Id", Id));
                DataSet ds = await _iSql.ExecuteProcedure("SP_GetData", param.ToArray());
                if (ds.Tables.Count > 0)
                {
                    return new result
                    {
                        status = true,
                        data = ds.Tables[0].AsEnumerable().Select(x => new
                        {
                            id = Convert.ToInt32(x["Id"]),
                            name = Convert.ToString(x["SectorName"]),
                        }).ToList()
                    };
                }
                else
                {
                    return new result
                    {
                        status = false,
                        message = "Something went wrong",
                    };
                }
            }
            catch (Exception ex)
            {
                return new result
                {
                    status = false,
                    message = "Something went wrong",
                };
            }

        }
        public async Task<result> DDL_SubSectorAsync(int? Id)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Action", "DDL_SubSector"));
                param.Add(new SqlParameter("@Id", Id));
                DataSet ds = await _iSql.ExecuteProcedure("SP_GetData", param.ToArray());
                if (ds.Tables.Count > 0)
                {
                    return new result
                    {
                        status = true,
                        data = ds.Tables[0].AsEnumerable().Select(x => new
                        {
                            id = Convert.ToInt32(x["Id"]),
                            name = Convert.ToString(x["SubSectorName"]),
                        }).ToList()
                    };
                }
                else
                {
                    return new result
                    {
                        status = false,
                        message = "Something went wrong",
                    };
                }
            }
            catch (Exception ex)
            {
                return new result
                {
                    status = false,
                    message = "Something went wrong",
                };
            }

        }       
        public async Task<result> DDL_GoalAsync()
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Action", "DDL_Goal"));
                DataSet ds = await _iSql.ExecuteProcedure("SP_GetData", param.ToArray());
                if (ds.Tables.Count > 0)
                {
                    return new result
                    {
                        status = true,
                        data = ds.Tables[0].AsEnumerable().Select(x => new
                        {
                            id = Convert.ToInt32(x["Id"]),
                            name = Convert.ToString(x["Goal"]),
                        }).ToList()
                    };
                }
                else
                {
                    return new result
                    {
                        status = false,
                        message = "Something went wrong",
                    };
                }
            }
            catch (Exception ex)
            {
                return new result
                {
                    status = false,
                    message = "Something went wrong",
                };
            }

        }
        public async Task<result> DDL_TargetAsync(int? Id)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Action", "DDL_Target"));
                param.Add(new SqlParameter("@Id", Id));
                DataSet ds = await _iSql.ExecuteProcedure("SP_GetData", param.ToArray());
                if (ds.Tables.Count > 0)
                {
                    return new result
                    {
                        status = true,
                        data = ds.Tables[0].AsEnumerable().Select(x => new
                        {
                            id = Convert.ToInt32(x["Id"]),
                            name =Convert.ToString(x["Target"]),
                        }).ToList()
                    };
                }
                else
                {
                    return new result
                    {
                        status = false,
                        message = "Something went wrong",
                    };
                }
            }
            catch (Exception ex)
            {
                return new result
                {
                    status = false,
                    message = "Something went wrong",
                };
            }

        }

        public async Task<result> DDL_DistrictAsync()
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Action", "DDL_District"));
                DataSet ds = await _iSql.ExecuteProcedure("SP_GetData", param.ToArray());
                if (ds.Tables.Count > 0)
                {
                    return new result
                    {
                        status = true,
                        data = ds.Tables[0].AsEnumerable().Select(x => new
                        {
                            id = Convert.ToInt32(x["Id"]),
                            name = Convert.ToString(x["Name"]),
                        }).ToList()
                    };
                }
                else
                {
                    return new result
                    {
                        status = false,
                        message = "Something went wrong",
                    };
                }
            }
            catch (Exception ex)
            {
                return new result
                {
                    status = false,
                    message = "Something went wrong",
                };
            }

        }
        public async Task<result> DDL_BlockAsync(int? Id)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Action", "DDL_Block"));
                param.Add(new SqlParameter("@Id", Id));
                DataSet ds = await _iSql.ExecuteProcedure("SP_GetData", param.ToArray());
                if (ds.Tables.Count > 0)
                {
                    return new result
                    {
                        status = true,
                        data = ds.Tables[0].AsEnumerable().Select(x => new
                        {
                            id = Convert.ToInt32(x["Id"]),
                            name = Convert.ToString(x["Name"]),
                        }).ToList()
                    };
                }
                else
                {
                    return new result
                    {
                        status = false,
                        message = "Something went wrong",
                    };
                }
            }
            catch (Exception ex)
            {
                return new result
                {
                    status = false,
                    message = "Something went wrong",
                };
            }

        }
        public async Task<result> DDL_CityAsync(int? Id)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Action", "DDL_City"));
                param.Add(new SqlParameter("@Id", Id));
                DataSet ds = await _iSql.ExecuteProcedure("SP_GetData", param.ToArray());
                if (ds.Tables.Count > 0)
                {
                    return new result
                    {
                        status = true,
                        data = ds.Tables[0].AsEnumerable().Select(x => new
                        {
                            id = Convert.ToInt32(x["Id"]),
                            name = Convert.ToString(x["Name"]),
                        }).ToList()
                    };
                }
                else
                {
                    return new result
                    {
                        status = false,
                        message = "Something went wrong",
                    };
                }
            }
            catch (Exception ex)
            {
                return new result
                {
                    status = false,
                    message = "Something went wrong",
                };
            }

        }

        public async Task<result> DDL_UNSectorAsync()
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Action", "DDL_UNSector"));
                DataSet ds = await _iSql.ExecuteProcedure("SP_GetData", param.ToArray());
                if (ds.Tables.Count > 0)
                {
                    return new result
                    {
                        status = true,
                        data = ds.Tables[0].AsEnumerable().Select(x => new
                        {
                            id = Convert.ToInt32(x["Id"]),
                            name = Convert.ToString(x["Name"]),
                        }).ToList()
                    };
                }
                else
                {
                    return new result
                    {
                        status = false,
                        message = "Something went wrong",
                    };
                }
            }
            catch (Exception ex)
            {
                return new result
                {
                    status = false,
                    message = "Something went wrong",
                };
            }

        }

        public async Task<result> DDL_NatureOfSupportAsync()
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Action", "DDL_NatureOfSupport"));
                DataSet ds = await _iSql.ExecuteProcedure("SP_GetData", param.ToArray());
                if (ds.Tables.Count > 0)
                {
                    return new result
                    {
                        status = true,
                        data = ds.Tables[0].AsEnumerable().Select(x => new
                        {
                            id = Convert.ToInt32(x["Id"]),
                            name = Convert.ToString(x["Name"]),
                        }).ToList()
                    };
                }
                else
                {
                    return new result
                    {
                        status = false,
                        message = "Something went wrong",
                    };
                }
            }
            catch (Exception ex)
            {
                return new result
                {
                    status = false,
                    message = "Something went wrong",
                };
            }

        }
        public async Task<result> DDL_SubNatureOfSupportAsync(int? Id)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Action", "DDL_SubNatureOfSupport"));
                param.Add(new SqlParameter("@Id", Id));
                DataSet ds = await _iSql.ExecuteProcedure("SP_GetData", param.ToArray());
                if (ds.Tables.Count > 0)
                {
                    return new result
                    {
                        status = true,
                        data = ds.Tables[0].AsEnumerable().Select(x => new
                        {
                            id = Convert.ToInt32(x["Id"]),
                            name = Convert.ToString(x["Name"]),
                        }).ToList()
                    };
                }
                else
                {
                    return new result
                    {
                        status = false,
                        message = "Something went wrong",
                    };
                }
            }
            catch (Exception ex)
            {
                return new result
                {
                    status = false,
                    message = "Something went wrong",
                };
            }

        }
    }
}
