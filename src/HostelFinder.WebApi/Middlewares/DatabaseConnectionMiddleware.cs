using HostelFinder.Infrastructure.Context;

namespace HostelFinder.WebApi.Middlewares
{
    public class DatabaseConnectionMiddleware
    {
        private readonly RequestDelegate _next;

        public DatabaseConnectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var dbContext = context.RequestServices.GetService<HostelFinderDbContext>();

                var canConnect = dbContext.Database.CanConnect();

                if (!canConnect)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Unable to connect to the database.");
                    return;
                }

                // Proceed with the next middleware in the pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync($"Database connection failed: {ex.Message}");
            }
        }
    }
}
