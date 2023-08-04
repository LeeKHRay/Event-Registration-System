namespace EventRegistrationSystem.Middlewares;

public class JqueryAjaxApiRequestMiddleware
{
    private readonly RequestDelegate _next;

    public JqueryAjaxApiRequestMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // check if it is a jquery ajax request
        if (context.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Call this Api using Jquery ajax");

            return; // not calling _next(context) will short-circuit the middleware pipeline
        }

        await _next(context);
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class JqueryAjaxApiRequestMiddlewareExtensions
{
    public static IApplicationBuilder UseJqueryAjaxApiRequest(this IApplicationBuilder app)
    {
        // execute the middleware if the path starts with "/api"
        return app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"),
            app => app.UseMiddleware<JqueryAjaxApiRequestMiddleware>());
    }
}
