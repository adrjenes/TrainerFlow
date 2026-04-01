using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using TrainerFlow.Api.Exceptions;
using TrainerFlow.Shared.Exceptions;

namespace TrainerFlow.Tests.Unit.Api.Exceptions;

public sealed class GlobalExceptionHandlerTests
{
    [Fact]
    public async Task TryHandleAsync_ShouldReturn400WithValidationErrors_WhenExceptionIsValidationException()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GlobalExceptionHandler>>();
        var problemDetailsServiceMock = new Mock<IProblemDetailsService>();
        var environmentMock = CreateEnvironmentMock(Environments.Production);

        ProblemDetailsContext? capturedContext = null;

        problemDetailsServiceMock
            .Setup(x => x.WriteAsync(It.IsAny<ProblemDetailsContext>()))
            .Callback<ProblemDetailsContext>(context => capturedContext = context)
            .Returns(ValueTask.CompletedTask);

        var handler = new GlobalExceptionHandler(
            loggerMock.Object,
            problemDetailsServiceMock.Object,
            environmentMock.Object);

        var httpContext = new DefaultHttpContext
        {
            TraceIdentifier = "trace-validation"
        };

        var exception = new ValidationException(
        [
            new ValidationFailure("Email", "Email is required."),
            new ValidationFailure("Email", "Email is invalid."),
            new ValidationFailure("FirstName", "First name is required.")
        ]);

        // Act
        var result = await handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        httpContext.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

        problemDetailsServiceMock.Verify(
            x => x.WriteAsync(It.IsAny<ProblemDetailsContext>()),
            Times.Once);

        capturedContext.Should().NotBeNull();

        var problemDetails = capturedContext!.ProblemDetails;
        problemDetails.Status.Should().Be(StatusCodes.Status400BadRequest);
        problemDetails.Title.Should().Be("One or more validation errors occurred.");
        problemDetails.Detail.Should().BeNull();

        problemDetails.Extensions.Should().ContainKey("traceId");
        problemDetails.Extensions["traceId"].Should().Be("trace-validation");

        problemDetails.Extensions.Should().ContainKey("errors");

        var errors = problemDetails.Extensions["errors"];
        errors.Should().BeAssignableTo<IReadOnlyDictionary<string, string[]>>();

        var validationErrors = (IReadOnlyDictionary<string, string[]>)errors;
        validationErrors.Should().ContainKey("Email");
        validationErrors["Email"].Should().BeEquivalentTo(
        [
            "Email is required.",
            "Email is invalid."
        ]);

        validationErrors.Should().ContainKey("FirstName");
        validationErrors["FirstName"].Should().BeEquivalentTo(
        [
            "First name is required."
        ]);
    }

    [Fact]
    public async Task TryHandleAsync_ShouldReturn404_WhenExceptionIsNotFoundException()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GlobalExceptionHandler>>();
        var problemDetailsServiceMock = new Mock<IProblemDetailsService>();
        var environmentMock = CreateEnvironmentMock(Environments.Production);

        ProblemDetailsContext? capturedContext = null;

        problemDetailsServiceMock
            .Setup(x => x.WriteAsync(It.IsAny<ProblemDetailsContext>()))
            .Callback<ProblemDetailsContext>(context => capturedContext = context)
            .Returns(ValueTask.CompletedTask);

        var handler = new GlobalExceptionHandler(
            loggerMock.Object,
            problemDetailsServiceMock.Object,
            environmentMock.Object);

        var httpContext = new DefaultHttpContext
        {
            TraceIdentifier = "trace-not-found"
        };

        var exception = new NotFoundException("Offer not found.");

        // Act
        var result = await handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        httpContext.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        problemDetailsServiceMock.Verify(
            x => x.WriteAsync(It.IsAny<ProblemDetailsContext>()),
            Times.Once);

        capturedContext.Should().NotBeNull();

        var problemDetails = capturedContext!.ProblemDetails;
        problemDetails.Status.Should().Be(StatusCodes.Status404NotFound);
        problemDetails.Title.Should().Be("The requested resource was not found.");
        problemDetails.Detail.Should().BeNull();

        problemDetails.Extensions.Should().ContainKey("traceId");
        problemDetails.Extensions["traceId"].Should().Be("trace-not-found");
        problemDetails.Extensions.Should().NotContainKey("errors");
    }

    [Fact]
    public async Task TryHandleAsync_ShouldReturn500_WhenExceptionIsUnhandled()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GlobalExceptionHandler>>();
        var problemDetailsServiceMock = new Mock<IProblemDetailsService>();
        var environmentMock = CreateEnvironmentMock(Environments.Production);

        ProblemDetailsContext? capturedContext = null;

        problemDetailsServiceMock
            .Setup(x => x.WriteAsync(It.IsAny<ProblemDetailsContext>()))
            .Callback<ProblemDetailsContext>(context => capturedContext = context)
            .Returns(ValueTask.CompletedTask);

        var handler = new GlobalExceptionHandler(
            loggerMock.Object,
            problemDetailsServiceMock.Object,
            environmentMock.Object);

        var httpContext = new DefaultHttpContext
        {
            TraceIdentifier = "trace-500"
        };

        var exception = new Exception("Unexpected error.");

        // Act
        var result = await handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        httpContext.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

        problemDetailsServiceMock.Verify(
            x => x.WriteAsync(It.IsAny<ProblemDetailsContext>()),
            Times.Once);

        capturedContext.Should().NotBeNull();

        var problemDetails = capturedContext!.ProblemDetails;
        problemDetails.Status.Should().Be(StatusCodes.Status500InternalServerError);
        problemDetails.Title.Should().Be("An unexpected error occurred.");
        problemDetails.Detail.Should().BeNull();

        problemDetails.Extensions.Should().ContainKey("traceId");
        problemDetails.Extensions["traceId"].Should().Be("trace-500");
        problemDetails.Extensions.Should().NotContainKey("errors");
    }

    [Fact]
    public async Task TryHandleAsync_ShouldIncludeExceptionMessageInDetail_WhenEnvironmentIsDevelopment()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GlobalExceptionHandler>>();
        var problemDetailsServiceMock = new Mock<IProblemDetailsService>();
        var environmentMock = CreateEnvironmentMock(Environments.Development);

        ProblemDetailsContext? capturedContext = null;

        problemDetailsServiceMock
            .Setup(x => x.WriteAsync(It.IsAny<ProblemDetailsContext>()))
            .Callback<ProblemDetailsContext>(context => capturedContext = context)
            .Returns(ValueTask.CompletedTask);

        var handler = new GlobalExceptionHandler(
            loggerMock.Object,
            problemDetailsServiceMock.Object,
            environmentMock.Object);

        var httpContext = new DefaultHttpContext
        {
            TraceIdentifier = "trace-dev"
        };

        var exception = new Exception("Developer-only detail.");

        // Act
        var result = await handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        problemDetailsServiceMock.Verify(
            x => x.WriteAsync(It.IsAny<ProblemDetailsContext>()),
            Times.Once);

        capturedContext.Should().NotBeNull();
        capturedContext!.ProblemDetails.Detail.Should().Be("Developer-only detail.");
        capturedContext.ProblemDetails.Extensions["traceId"].Should().Be("trace-dev");
    }

    private static Mock<IHostEnvironment> CreateEnvironmentMock(string environmentName)
    {
        var environmentMock = new Mock<IHostEnvironment>();

        environmentMock
            .SetupGet(x => x.EnvironmentName)
            .Returns(environmentName);

        return environmentMock;
    }
}