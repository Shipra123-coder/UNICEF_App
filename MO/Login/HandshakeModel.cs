using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Login
{
    public class HandshakeModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public SSOUserDetail SSOUserDetail { get; set; }

    }
    public class SSOdeatilCredentials
    {
        public string BaseUrl { get; set; }
        public string AppName { get; set; }
        public string WebServiceUser { get; set; }
        public string WebServicePwd { get; set; }
        public string BackTOSSO { get; set; }
        public string SSOLogOut { get; set; }
    }

    public class SSOSessionMO
    {
        public string sAMAccountName { get; set; }
        public List<string> Roles { get; set; }
        public string OldSSOIDs { get; set; }
        public string UserType { get; set; }
        public object DelegateBy { get; set; }
    }

    public class SessionLogin
    {
        public string UserName { get; set; }
        public string Flag { get; set; }
        public int UserId { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string UserType { get; set; }
        public string Name { get; set; }
        public int RoleID { get; set; }
        public string Password { get; set; }
        public string Email_ID { get; set; }
        public string DepartmentId { get; set; }
        public string LoginId { get; set; }
        public string WELCOMENAME { get; set; }
        public string SSOToken { get; set; }

    }

}
