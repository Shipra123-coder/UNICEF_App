using DL;
using MO.Common;
using MO.ProfileUser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Common
{
    public class Permission : IPermission
    {
        #region Properties
        private readonly ISQLHelper _iSql;
        #endregion
        #region Constructor
        public Permission(ISQLHelper iSql)
        {
            _iSql = iSql;
        }
        #endregion
        public async Task<MenuPermissionMO?> GetPagePermissionAsync(int groupId, string url)
        {
            MenuPermissionMO _res = new MenuPermissionMO();
            try
            {
                List<SqlParameter> param = new List<SqlParameter>
                {
                    new SqlParameter("@Action", "GetPagePermission"),
                    new SqlParameter("@GroupId", groupId),
                    new SqlParameter("@Url", url)
                };
                DataSet ds = await _iSql.ExecuteProcedure("SP_ManagePermission", param.ToArray());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];

                    return _res = new MenuPermissionMO
                    {
                        PermissionId = row["PermissionId"] != DBNull.Value ? Convert.ToInt32(row["PermissionId"]) : 0,
                        MenuId = row["MenuId"] != DBNull.Value ? Convert.ToInt32(row["MenuId"]) : 0,
                        GroupId = row["GroupId"] != DBNull.Value ? Convert.ToInt32(row["GroupId"]) : 0,
                        UserId = row["UserId"] != DBNull.Value ? Convert.ToInt32(row["UserId"]) : 0,
                        IsActive = row["IsActive"] != DBNull.Value && Convert.ToBoolean(row["IsActive"]),
                        Url = row["Url"] != DBNull.Value ? row["Url"].ToString() : string.Empty,
                        CanAdd = row["CanAdd"] != DBNull.Value && Convert.ToBoolean(row["CanAdd"]),
                        CanList = row["CanList"] != DBNull.Value && Convert.ToBoolean(row["CanList"]),
                        CanEdit = row["CanEdit"] != DBNull.Value && Convert.ToBoolean(row["CanEdit"]),
                        CanDelete = row["CanDelete"] != DBNull.Value && Convert.ToBoolean(row["CanDelete"]),
                        CanActiveDeactive = row["CanActiveDeactive"] != DBNull.Value && Convert.ToBoolean(row["CanActiveDeactive"])
                    };
                }

                return new MenuPermissionMO();
            }
            catch(Exception ex)
            {

            }
            return new MenuPermissionMO();
        }

    }
}
