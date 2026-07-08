using MO.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BL.Account
{
    public interface ILogin
    {
        ClaimsIdentity claims(loginModels.UserDetailsData ud);
        Task<loginModels.login_result> userAuthenticate(loginModels.loginCredentials lc);
        Task<loginModels.loginResult> SDG_Check_User_Login_CredentialSSOID(loginModels.loginCredentials lc);
        ClaimsIdentity ssoClaims(loginModels.userData ud, string ssoToken);
        Task<string> GetAPIResult(string methodURL, string ssoToken);
        Task<string> userAuthenticate_getPassword(string username);
    }
}
