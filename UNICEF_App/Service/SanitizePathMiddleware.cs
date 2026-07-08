namespace UNICEF_App.Service
{
    public class SanitizePathMiddleware
    {
        private readonly RequestDelegate _next;

        public SanitizePathMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                var path = context.Request.Path.Value ?? "";

                // Debug Log (to see the path)
                Console.WriteLine("Requested Path: " + path);

                // Check if path contains *
                if (path.Contains('*'))
                {
                    Console.WriteLine("Found *, sanitizing path...");
                    context.Request.Path = path.Replace("*", "_");
                }                
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine("Middleware Error: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);

                // Optionally rethrow for pipeline to handle
                throw;
            }
        }

        //public async Task Invoke(HttpContext context)
        //{
        //    var path = context.Request.Path.Value;

        //    // Example: Sanitize by removing '*' character from the path
        //    if (path.Contains('*'))
        //    {
        //        context.Request.Path = path.Replace("*", "_");
        //    }

        //    await _next(context);
        //}
    }
}
