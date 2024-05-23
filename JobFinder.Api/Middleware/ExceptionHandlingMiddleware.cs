using System.Net;
using System.Text.Json;

namespace JobFinder.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = ex switch
        {
            RestSharpException rse => (int)rse.StatusCode,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var response = new
        {
            StatusCode = context.Response.StatusCode,
            Message = ex.Message,
            Detailed = ex.InnerException?.Message
        };

        var jsonResponse = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(jsonResponse);
    }
}

public class RestSharpException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public RestSharpException(string message, HttpStatusCode statusCode)
        : base(message)
    {
        StatusCode = statusCode;
    }
}
