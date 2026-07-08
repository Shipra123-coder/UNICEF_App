namespace UNICEF_App.Service
{
    public class AccessDeniedMiddleware
    {
        private readonly RequestDelegate _next;

        public AccessDeniedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == 404) // Access Denied
            {
                context.Response.Redirect("/StatusError/403");
            }
        }
    }
}
