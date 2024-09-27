using System.Net;
using System.Text.Json;

namespace Api.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next, bool showRawError, ILogger<ExceptionHandlerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception error)
        {
            await HandleExceptionAsync(context, error);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception error)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        logger.LogError(error, error.Message);
        string _error = showRawError ? error.Message : "";
        string _message = "Internal Server Error";

        response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var result = JsonSerializer.Serialize(new { message = _message, error = _error });
        await response.WriteAsync(result);
    }
}
