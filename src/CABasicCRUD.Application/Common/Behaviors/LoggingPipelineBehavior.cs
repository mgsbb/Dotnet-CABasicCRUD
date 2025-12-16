using CABasicCRUD.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CABasicCRUD.Application.Common.Behaviors;

public class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;

    public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "Request start: {@RequestName} {@DateTimeUtc}",
            typeof(TRequest).Name,
            DateTime.UtcNow
        );

        var result = await next();

        if (result.IsFailure)
        {
            _logger.LogInformation(
                "Request failure: {@RequestName} {@Error} {@DateTimeUtc}",
                typeof(TRequest).Name,
                result.Error?.Code,
                DateTime.UtcNow
            );
        }

        _logger.LogInformation(
            "Request end: {@RequestName} {@DateTimeUtc}",
            typeof(TRequest).Name,
            DateTime.UtcNow
        );

        return result;
    }
}
