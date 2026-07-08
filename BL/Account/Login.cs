using DL;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MO.Login;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace BL.Account
{
    public class Login : ILogin
    {
        #region Properties
        private readonly ISQLHelper _iSql;
        public static readonly string securityKey = "FB9D4558-18F1-46AB-B6F0-E55CFF8C4231";
        private readonly SSOdeatilCredentials _appSettings;
        #endregion

        #region Constructor
        public Login(ISQLHelper iSql, IOptions<SSOdeatilCredentials> appSettings)
        {
            _iSql = iSql;
            _appSettings = appSettings.Value;

        }
        #endregion

        public async Task<loginModels.login_result> userAuthenticate(loginModels.loginCredentials lc)
        {
            try
            {
                var lr = new loginModels.login_result();
                if (!string.IsNullOrEmpty(lc.Username) && string.IsNullOrEmpty(lc.Password))
                {

                    return new loginModels.login_result()
                    {
                        isAuthenticate = false,
                        message = "Please enter all required details"

                    };

                }
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@ssoid", lc.Username));
                param.Add(new SqlParameter("@Password", lc.Password));
                param.Add(new SqlParameter("@SecrityKey", securityKey));
                DataSet ds = await _iSql.ExecuteProcedure("sp_Login", param.ToArray());
                if (Convert.ToInt32(ds.Tables[0].Rows[0]["StatusCode"]) == 1)
                {
                    return new loginModels.login_result()
                    {
                        isAuthenticate = true,
                        message = "Login Successfully",
                        user = ds.Tables[0].AsEnumerable().Select(x => new loginModels.UserDetailsData
                        {                          
                            userId = x["UserId"] == DBNull.Value ? 0 : Convert.ToInt32(x["UserId"]),
                            displayname = x["DisplayName"] == DBNull.Value ? "" : Convert.ToString(x["DisplayName"]),
                            name = x["Name"] == DBNull.Value ? "" : Convert.ToString(x["Name"]),
                            ssoid = x["SSOID"] == DBNull.Value ? "" : Convert.ToString(x["SSOID"]),
                            agencyId = x["AgencyId"] == DBNull.Value ? 0 : Convert.ToInt32(x["AgencyId"]),
                            agencyName = x["AgencyName"] == DBNull.Value ? "" : Convert.ToString(x["AgencyName"]),
                            agencyCode = x["AgencyCode"] == DBNull.Value ? "" : Convert.ToString(x["AgencyCode"]),
                            //districtId = x["DistrictId"] == DBNull.Value ? 0 : Convert.ToInt32(x["DistrictId"]),
                            //districtName = x["District"] == DBNull.Value ? "" : Convert.ToString(x["District"]),
                            role = x["Role"] == DBNull.Value ? "" : Convert.ToString(x["Role"]),
                            roleId = x["RoleId"] == DBNull.Value ? "" : Convert.ToString(x["RoleId"]),
                            department = x["DepartmentName"] == DBNull.Value ? "" : Convert.ToString(x["DepartmentName"]),
                            departmentId = x["DepartmentId"] == DBNull.Value ? 0 : Convert.ToInt32(x["DepartmentId"]),
                            //userType = x["UserType"] == DBNull.Value ? "" : Convert.ToString(x["UserType"]),
                            userLevel = x["UserLevel"] == DBNull.Value ? "" : Convert.ToString(x["UserLevel"]),
                            groupId = x["GroupId"] == DBNull.Value ? 0 : Convert.ToInt32(x["GroupId"]),
                            FinancialYear = x["FinancialYear"] == DBNull.Value ? 0 : Convert.ToInt32(x["FinancialYear"]),
                            FinancialYeartext = x["FinancialYearText"] == DBNull.Value ? "" : Convert.ToString(x["FinancialYeartext"]),
                        }).FirstOrDefault(),
                    };
                }
                else
                {
                    return new loginModels.login_result()
                    {
                        isAuthenticate = false,
                        message = "Login failed. Username or password is incorrect!",
                    };
                }
            }
            catch (Exception ex)
            {
                return new loginModels.login_result()
                {
                    isAuthenticate = false,
                    message = "Something wents wrong!",

                };
            }
        }
        public ClaimsIdentity claims(loginModels.UserDetailsData ud)
        {
            ClaimsIdentity identity = null;
            identity = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name,ud.name),
            new Claim(ClaimTypes.Role,ud.role),            
            new Claim("UserId",ud.userId.ToString()),
            new Claim("UserName",ud.name.ToString()),
            //new Claim("DistrictId",ud.districtId.ToString()),
            //new Claim("DistrictName",ud.districtName.ToString()),
            new Claim("AgencyId",ud.agencyId.ToString()),
            new Claim("AgencyName",ud.agencyName.ToString()),
            new Claim("agencyCode",ud.agencyCode.ToString()),
            new Claim("RoleId",ud.roleId.ToString()),
            new Claim("Role",ud.role.ToString()),
            new Claim("DepartmentId",ud.departmentId.ToString()),
            new Claim("Department",ud.department.ToString()),
            new Claim("UserLevel",ud.userLevel.ToString()),
            new Claim("GroupId",ud.groupId.ToString()),
            new Claim("FinancialYear",ud.FinancialYear.ToString()),
            new Claim("Financialtext",ud.FinancialYeartext.ToString()),
        }, CookieAuthenticationDefaults.AuthenticationScheme);
            return identity;
        }
        public ClaimsIdentity ssoClaims(loginModels.userData ud, string ssoToken)
        {
            ClaimsIdentity identity = null;
            identity = new ClaimsIdentity(new[]
            {
           new Claim(ClaimTypes.Name,ud.name),
           new Claim(ClaimTypes.Role,ud.roleName),
           new Claim("UserId",ud.userId.ToString()),
           new Claim("UserName",ud.name),
           new Claim("RoleId",ud.roleId.ToString()),
           new Claim("SSOID",ud.ssoid.ToString()),
           //new Claim("DepartmentId",ud.deptDistId.ToString()),
           new Claim("WELCOMENAME",ud.welcomeName.ToString()),
           new Claim("RoleName",ud.roleName.ToString()),
           new Claim("ssoToken",ssoToken),
           }, CookieAuthenticationDefaults.AuthenticationScheme);
            return identity;
        }
        public async Task<loginModels.loginResult> SDG_Check_User_Login_CredentialSSOID(loginModels.loginCredentials lc)
        {
            var lr = new loginModels.loginResult();
            if (!string.IsNullOrEmpty(lc.Username) && string.IsNullOrEmpty(lc.Password))
            {

                return new loginModels.loginResult()
                {
                    isAuthenticate = false,
                    message = "Please enter all required details"

                };
            }
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                //param.Add(new SqlParameter("@UserName", lc.ssoid));
                param.Add(new SqlParameter("@Flag", "Login"));
                DataSet ds = await _iSql.ExecuteProcedure("SP_ManageLoginCredentialBYSSOID", param.ToArray());
                //DataTable dataTable = adminDal.SDG_Check_User_Login_CredentialSSOID(UserDetails_Entity);
                if (ds.Tables.Count > 0)
                {

                    return new loginModels.loginResult()
                    {
                        isAuthenticate = true,
                        message = "Login Successfully",
                        user = ds.Tables[0].AsEnumerable().Select(x => new loginModels.userData
                        {
                            name = x["Name"] == DBNull.Value ? "" : Convert.ToString(x["Name"]),
                            mobile = x["Mobile"] == DBNull.Value ? "" : Convert.ToString(x["Mobile"]),
                            password = x["Password"] == DBNull.Value ? "" : Convert.ToString(x["Password"]),
                            roleId = x["RoleID"] == DBNull.Value ? 0 : Convert.ToInt32(x["RoleID"]),
                            userId = x["UserId"] == DBNull.Value ? 0 : Convert.ToInt32(x["UserId"]),
                            email = x["EMAIL_ID"] == DBNull.Value ? "" : Convert.ToString(x["EMAIL_ID"]),
                            deptDistId = x["DeptDist_id"] == DBNull.Value ? "" : Convert.ToString(x["DeptDist_id"]),
                            roleName = x["roleName"] == DBNull.Value ? "" : Convert.ToString(x["roleName"]),
                            welcomeName = x["WELCOMENAME"] == DBNull.Value ? "" : Convert.ToString(x["WELCOMENAME"]),
                            ssoid = x["SSOID"] == DBNull.Value ? "" : Convert.ToString(x["SSOID"]),
                        }).FirstOrDefault(),

                    };
                }
                else
                {
                    return new loginModels.loginResult()
                    {
                        isAuthenticate = false,
                        message = "Login failed. Username or password is incorrect!",

                    };
                }
            }
            catch (Exception ex)
            {
                return new loginModels.loginResult()
                {
                    isAuthenticate = false,
                    message = "Something wents wrong!",

                };
            }
        }
        public async Task<string> GetAPIResult(string methodURL, string ssoToken)
        {
            string wsUsername = _appSettings.WebServiceUser;
            string wsPassword = _appSettings.WebServicePwd;
            //methodURL = "https://sso.rajasthan.gov.in//SSORESTNEW/Profile/" + wsUsername;
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, methodURL))
                {
                    request.Headers.Add("SSO-TOKEN", ssoToken);
                    request.Headers.Add("Authorization", "Basic " +
                    Convert.ToBase64String(Encoding.Default.GetBytes(wsUsername + ":" + wsPassword)));
                    using (var response = await client.SendAsync(request))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            return await response.Content.ReadAsStringAsync();
                        }
                    }
                    return default(string);
                }
            }
            //}
        }
        public async Task<string> userAuthenticate_getPassword(string username) {
            try
            {
                var lr = new loginModels.login_result();                
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Username", username));                
                DataSet ds = await _iSql.ExecuteProcedure("sp_Login_getPassword", param.ToArray());
                if (Convert.ToInt32(ds.Tables[0].Rows[0]["StatusCode"]) == 1)
                {
                    return ds.Tables[0].Rows[0]["Password"].ToString();
                }
                else
                {
                    return "";
                    //return new loginModels.login_result()
                    //{
                    //    isAuthenticate = false,
                    //    message = "Login failed. Username or password is incorrect!",
                    //};
                }
            }
            catch (Exception ex)
            {
                return "";
                //return new loginModels.login_result()
                //{
                //    isAuthenticate = false,
                //    message = "Something wents wrong!",

                //};
            }
        }
    }
}
