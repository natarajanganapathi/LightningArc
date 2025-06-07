namespace Falcon.WebApi.Abstractions.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception has occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var problemDetails = new ProblemDetails
        {
            Title = "An unexpected error occurred.",
            Status = (int)HttpStatusCode.InternalServerError,
            Detail = exception.Message,
            Instance = context.Request.Path
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = problemDetails.Status.Value;

        var response = JsonSerializer.Serialize(problemDetails);
        return context.Response.WriteAsync(response);
    }
}
