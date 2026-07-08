
using System.Security.Claims;
using System.Security.Principal;

namespace UNICEF_App
{
   
        public static class IdentityExtensions
        {
            public static string GetDetailOf(this IIdentity identity, string claimName)
            {
                string _returnString = string.Empty;
                var claimsIndentity = ((ClaimsIdentity)identity);
                var userClaims = claimsIndentity.Claims;
                foreach (var claim in userClaims)
                {
                    var cType = claim.Type;
                    var cValue = claim.Value;
                    if (claimName == cType)
                    {
                        _returnString = (cValue != null) ? cValue : string.Empty;
                    }
                }
                return _returnString;
            }
            public static void AddOrReplace(this IDictionary<string, object> DICT, string key, object value)
            {
                if (DICT.ContainsKey(key))
                    DICT[key] = value;
                else
                    DICT.Add(key, value);
            }
        }
    
}
