using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CABasicCRUD.Presentation.WebApi.Middlewares;

public sealed class GlobalExceptionHandlingMiddleware(
    ILogger<GlobalExceptionHandlingMiddleware> logger
) : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unhandled exception occurred. Method: {Method}, Path: {Path},  TraceId: {TraceId}",
                context.Request.Method,
                context.Request.Path,
                context.TraceIdentifier
            );

            ProblemDetailsFactory factory =
                context.RequestServices.GetRequiredService<ProblemDetailsFactory>();

            ProblemDetails problem = factory.CreateProblemDetails(
                context,
                statusCode: StatusCodes.Status500InternalServerError,
                // type: "A server error occurred",
                // title: "A server error occurred",
                detail: "An internal server error occurred.",
                instance: context.Request.Path
            );

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
