using System.Diagnostics;
using Serilog.Context;

namespace SwipeLab.Middleware;

public class ContextLoggingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        using (LogContext.PushProperty("TraceId", Activity.Current?.TraceId.ToString()))
        using (LogContext.PushProperty("SpanId", Activity.Current?.SpanId.ToString()))
        {
            await next.Invoke(context);
        }
    }
}