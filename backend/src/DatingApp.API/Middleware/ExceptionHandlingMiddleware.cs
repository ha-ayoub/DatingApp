using DatingApp.Application.Common.Exceptions;
using System.Text.Json;

namespace DatingApp.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try { await next(context); }
        catch (Exception ex) { await HandleExceptionAsync(context, ex); }
    }

    private async Task HandleExceptionAsync(HttpContext ctx, Exception ex)
    {
        logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
        var (statusCode, title, errors) = ex switch
        {
            ValidationException ve => (400, "Validation Error", (object)ve.Errors),
            NotFoundException nfe  => (404, nfe.Message, (object)new { }),
            UnauthorizedException ue => (401, ue.Message, (object)new { }),
            ConflictException ce   => (409, ce.Message, (object)new { }),
            _                      => (500, "An unexpected error occurred.", (object)new { })
        };
        ctx.Response.ContentType = "application/json";
        ctx.Response.StatusCode = statusCode;
        await ctx.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            status = statusCode, title, errors, traceId = ctx.TraceIdentifier
        }));
    }
}
