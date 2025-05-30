using System.Text;

namespace SwipeLab.Middleware
{
    public class RequestBodyLoggerMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context, ILogger<RequestBodyLoggerMiddleware> logger)
        {
            if (context.Request.ContentLength.HasValue && context.Request.ContentLength > 0)
            {
                string requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
                if (!string.IsNullOrEmpty(requestBody))
                {
                    logger.LogInformation("The request body for path {Path} is {@Body}", context.Request.Path, requestBody);
                }

                byte[] requestData = Encoding.UTF8.GetBytes(requestBody);
                context.Request.Body = new MemoryStream(requestData);
            }

            await next.Invoke(context);
        }
    }
}
