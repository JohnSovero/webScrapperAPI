namespace backend.Middleware
{
    public class ValidationMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.QueryString.HasValue)
            {
                if (string.IsNullOrEmpty(context.Request.Query["name"]))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new { error = "El parámetro 'name' es obligatorio." });
                    return;
                }
            }
            await _next(context);
        }
    }
}