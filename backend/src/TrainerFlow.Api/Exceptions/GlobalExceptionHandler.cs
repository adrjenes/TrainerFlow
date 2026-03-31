using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TrainerFlow.Shared.Exceptions;

namespace TrainerFlow.Api.Exceptions;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IProblemDetailsService problemDetailsService, IHostEnvironment environment) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken _) 
    {
        var (statusCode, title) = exception switch
        {
            ValidationException => (StatusCodes.Status400BadRequest, "One or more validation errors occurred."),
            NotFoundException => (StatusCodes.Status404NotFound, "The requested resource was not found."),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };

        LogException(exception, statusCode, httpContext.TraceIdentifier);

        httpContext.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title
        };

        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions["errors"] = validationException.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(x => x.ErrorMessage).ToArray());
        }

        if (environment.IsDevelopment())
        {
            problemDetails.Detail = exception.Message;
        }

        await problemDetailsService.WriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails,
            Exception = exception
        });

        return true;
    }

    private void LogException(Exception exception, int statusCode, string traceId)
    {
        if (statusCode >= StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Unhandled exception occurred. TraceId: {TraceId}", traceId);
            return;
        }

        if (statusCode == StatusCodes.Status404NotFound)
        {
            logger.LogInformation(exception, "Resource not found. TraceId: {TraceId}", traceId);
            return;
        }

        logger.LogWarning(exception, "Request failed. TraceId: {TraceId}", traceId);
    }
}