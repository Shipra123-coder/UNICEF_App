using BL.AI;
using BL.PageAccessRequirement;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using UNICEF_App;
using UNICEF_App.Service;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews(); // Or AddRazorPages(), AddMvc()
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});
builder.Services.AddHttpClient<IOpenAIService, OpenAIService>();
// 🌟 ADDED HERE FOR SIZE LIMIT (Max 5MB File Upload Support)
// ========================================================================
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit= 52428800; // 5 MB बाइट्स में
});

builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 52428800; // 5 MB (IIS के लिए अनिवार्य)
});
// ========================================================================
// Add services to the container.
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.Name = ".AspNetCore.Antiforgery.GFtadfaI-b4";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // or Always for HTTPS
    options.Cookie.SameSite = SameSiteMode.Strict; // Adjust as necessary
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
    // Strict SameSite policy to prevent CSRF via cross-site cookies
    // Custom cookie name (optional)
    options.Cookie.Name = ".AspNetCore.Cookie";
    // Redirect paths (optional)
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/LogOut";

    // Expiration
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
});
builder.Services.AddSession(options =>
{

    options.Cookie.HttpOnly = true; // Prevent JavaScript from accessing the cookie
    options.Cookie.IsEssential = true; // Mark the cookie as essential   
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Ensure the cookie is only sent over HTTPS
    options.Cookie.SameSite = SameSiteMode.Strict; // Strict SameSite policy
    options.IdleTimeout = TimeSpan.FromMinutes(30);// Session timeout duration

});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    //options.Cookie.Name = ".AspNetCore.Cookies";
    //options.Cookie.HttpOnly = true;
    //options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // ? Cookie sent over HTTPS only
    //options.Cookie.SameSite = SameSiteMode.Strict;         // ? STRONGEST PROTECTION (use Lax only if needed)
    //                                                       // ? Prevent access via JavaScript
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/LogOut";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);     // Set session timeout
    options.SlidingExpiration = true;                      // Extend session on activity
    options.AccessDeniedPath = "/Account/AccessDenied";  // 403 redirect

});
//builder.Services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
builder.Services.Configure<CookieTempDataProviderOptions>(options =>
{
    options.Cookie.Name = ".AspNetCore.Mvc.CookieTempDataProvider";

    options.Cookie.HttpOnly = true;

    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

    options.Cookie.SameSite = SameSiteMode.Strict;

    options.Cookie.IsEssential = true;
});
#region[4. START : SERVICES INJECTOR]

new ServiceInject(builder.Services);
#endregion
// ✅ 🔹 ADD YOUR AUTHORIZATION + HTTP CONTEXT REGISTRATIONS HERE 🔹 ✅

//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//builder.Services.AddScoped<IAuthorizationHandler, PageAccessHandler>();

builder.Services.AddAuthorization(options =>
{
    // Policy for database-based permission check
    options.AddPolicy("PageAccess", policy =>
        policy.Requirements.Add(new PageAccessRequirement("")));
});

// ✅ END OF AUTHORIZATION SECTION
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/StatusError"); // Custom error page in production
    app.UseHsts();
}

app.UseHttpsRedirection();

// 🌟 1. स्टेटिक फाइल्स को सबसे ऊपर रखें (सिक्योरिटी हेडर क्लीनअप के साथ)
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        var headers = ctx.Context.Response.Headers;
        headers.Remove("Server");
        headers.Remove("X-Powered-By");
        // Security headers
        headers["X-Content-Type-Options"] = "nosniff";

        headers["Referrer-Policy"] = "no-referrer";

        headers["X-Frame-Options"] = "DENY";
        headers["Content-Security-Policy"] = "default-src 'none'; script-src 'self'; style-src 'self'; img-src 'self' data:; font-src 'self' data:; connect-src 'self'; object-src 'none'; base-uri 'self'; frame-ancestors 'self';require-trusted-types-for 'script';trusted-types default;";
    }
});

// 🌟 2. सिक्योरिटी और राउटिंग को पहले लोड करें
app.UseMiddleware<SecurityHeadersMiddleware>();
app.UseRouting();
app.UseSession();
// 🌟 3. ऑथेंटिकेशन और सेशन को राउटिंग के ठीक बाद रखें
app.UseAuthentication();
app.UseAuthorization();


// 🌟 4. कस्टम एक्सेस डिनाइड को ऑथेंटिकेशन के नीचे रखें
app.UseMiddleware<AccessDeniedMiddleware>();

// 🌟 5. सबसे महत्वपूर्ण सुधार: स्टेटस कोड रीडायरेक्ट को राउटिंग और ऑथेंटिकेशन के बाद रखें
// ताकि एरर पेज के पाथ (/StatusError/404) को राउटिंग इंजन आसानी से पहचान सके
app.UseStatusCodePagesWithRedirects("/StatusError/{0}");

// 6. एंडपॉइंट्स मैपिंग
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Land}/{id?}");

    // Ensure custom error pages are routed
    endpoints.MapControllerRoute(
        name: "error",
        pattern: "StatusError/{statusCode}",
        defaults: new { controller = "Error", action = "StatusError" }); // 💡 सुनिश्चित करें कि आपका एरर कंट्रोलर और एक्शन सही नाम से मैप्ड हो
});

app.Run();