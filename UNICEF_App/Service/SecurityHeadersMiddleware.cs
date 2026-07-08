using System.Net;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;

namespace UNICEF_App.Service
{
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Add("X-Download-Options", "noopen");
            context.Response.Headers.Add("X-Frame-Options", "DENY");          
            context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");           
            context.Response.Headers["Referrer-Policy"] = "no-referrer";
            // Remove the Server header
            context.Response.Headers.Remove("X-Powered-By");
            context.Response.Headers.Remove("Server");

            var nonce = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            context.Items["CSPNonce"] = nonce;

            context.Response.Headers.Remove("Content-Security-Policy");
            context.Response.Headers.Remove("Content-Security-Policy-Report-Only");

            context.Response.Headers["Content-Security-Policy"] = $@"
            default-src 'none';
            script-src 'nonce-{nonce}' 'strict-dynamic'; 
            script-src-attr 'none';
            style-src 'self' 'nonce-{nonce}';            
            style-src-attr 'none';
            img-src 'self' data:;
            font-src 'self' data:;
            connect-src 'self';
            form-action 'self' https://ssotest.rajasthan.gov.in;
            object-src 'none';
            base-uri 'self';
            frame-src 'self';
            frame-ancestors 'self';
            ".Replace(Environment.NewLine, " ");

            //require - trusted - types -for 'script';
            //trusted - types default;

            context.Response.Headers["Content-Security-Policy-Report-Only"] =
            "require-trusted-types-for 'script'; trusted-types default;";

            await _next(context);          
        }
    }
}
