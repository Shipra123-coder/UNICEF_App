using BL.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MO.Login;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace UNICEF_App.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ILogin _iLogin;
        private readonly SSOdeatilCredentials _appSettings;
        public AccountController(ILogger<AccountController> logger, ILogin login, IOptions<SSOdeatilCredentials> appSettings)
        {
            _logger = logger;
            _iLogin = login;
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Account/Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        private async Task<string> ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static string GenerateSalt(int size = 16)
        {
            byte[] saltBytes = new byte[size];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        private string HashPasswordWithSalt(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password + salt);
            var hash = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        #region Login
        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Login()
        {
            string nonce = Guid.NewGuid().ToString("N"); // or use random string           
            HttpContext.Session.SetString("loginNonce", nonce);

            ViewData["loginNonce"] = nonce; // Pass to Razor view
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(loginModels.loginCredentials lc)
        {
            try
            {
                string sessionNonce =
                    HttpContext.Session.GetString("loginNonce");              

                if (string.IsNullOrWhiteSpace(sessionNonce))
                {
                    return ReturnLoginError(lc, "Session expired. Please login again.");
                }

                if (!string.Equals(sessionNonce, lc.nonce, StringComparison.Ordinal))
                {
                    return ReturnLoginError(lc, "Invalid request.");
                }

                string realPassword =
                    await _iLogin.userAuthenticate_getPassword(lc.Username);

                if (string.IsNullOrWhiteSpace(realPassword))
                {
                    return ReturnLoginError(
                        lc,
                        "Invalid Username or Password!"
                    );
                }

                string serverHash =
                    HashPasswordWithSalt(realPassword, sessionNonce);

                if (!string.Equals(
                        lc.HashedPassword,
                        serverHash,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return ReturnLoginError(
                        lc,
                        "Invalid Username or Password!"
                    );
                }

                // Nonce one-time use
                HttpContext.Session.Remove("loginNonce");

                lc.Password = realPassword;

                var result =
                    await _iLogin.userAuthenticate(lc);

                if (!result.isAuthenticate)
                {
                    return ReturnLoginError(
                        lc,
                        "Invalid Username or Password!"
                    );
                }

                var identity =
                    _iLogin.claims(result.user);

                var principal =
                    new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal);

                if (result.user.groupId == 4)
                {
                    return RedirectToAction(
                        "Dashboard",
                        "Dashboard");
                }

                return RedirectToAction(
                    "ActivityManagement",
                    "Management");
            }
            catch (Exception)
            {
                return ReturnLoginError(
                    lc,
                    "Something went wrong!");
            }
        }

        private IActionResult ReturnLoginError(loginModels.loginCredentials lc, string message)
        {
            ModelState.AddModelError(
                string.Empty,
                message);

            string newNonce =
                Guid.NewGuid().ToString("N");

            HttpContext.Session.SetString(
                "loginNonce",
                newNonce);

            ViewData["loginNonce"] =
                newNonce;

            return View(lc);
        }
        #endregion


        #region Login_old

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Login_old()
        {
            string nonce = Guid.NewGuid().ToString("N"); // or use random string
            HttpContext.Session.SetString("loginNonce", nonce);

            ViewData["loginNonce"] = nonce; // Pass to Razor view
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login_old(loginModels.loginCredentials lc)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(lc); // or View(model) if not using AJAX partial
            //}
            var referer = Request.Headers["Referer"].ToString();
            try
            {

                string sessionNonce = lc.nonce;//HttpContext.Session.GetString("loginNonce");//lc.nonce;
                string clientPasswordHash = lc.HashedPassword;
                //get real Password
                string REALPASSWORD = await _iLogin.userAuthenticate_getPassword(lc.Username);
                string combined = REALPASSWORD + sessionNonce;
                string serverHash = HashPasswordWithSalt(REALPASSWORD, sessionNonce); // must match client hash

                //loginModels.login_result rs = new loginModels.login_result()
                //{
                //    isAuthenticate = false,
                //    isRedirect = false,
                //    status = false,
                //    message = "serverHash-"+serverHash+ "$" + clientPasswordHash +"$"+ sessionNonce +"Passwordread-"+ REALPASSWORD,
                //};
                //return Json(rs);
                if (clientPasswordHash != serverHash)
                {
                    loginModels.login_result rs = new loginModels.login_result()
                    {
                        isAuthenticate = false,
                        isRedirect = false,
                        status = false,
                        message = "Invalid Login !",
                    };
                    return Json(rs);
                }
                else
                {
                    lc.Password = REALPASSWORD;
                    var result = await _iLogin.userAuthenticate(lc);
                    if (result.isAuthenticate)
                    {
                        var identity = _iLogin.claims(result.user);
                        var principal = new ClaimsPrincipal(identity);
                        //HttpContext.Session.SetString("SessionSalt", GenerateSalt());
                        var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        result.isRedirect = true;
                        result.status = true;

                        if (result.user.groupId == 6 || result.user.groupId == 8)
                        {
                            result.redirectUrl = Url.Action("DistrictDashboard", "Dashboard");
                        }
                        else
                        {
                            result.redirectUrl = Url.Action("ActivityManagement", "Management"); // 👈 Add this
                        }
                        return Json(result);
                    }
                    else
                    {
                        result.isRedirect = false;
                        result.status = false;
                    }
                    return Json(result);
                }


            }
            catch (Exception ex)
            {
                loginModels.loginResult rs = new loginModels.loginResult()
                {
                    isAuthenticate = false,
                    message = "Invalid Login !",
                };
                return Json(rs);
            }
        }
        #endregion
    }
}
