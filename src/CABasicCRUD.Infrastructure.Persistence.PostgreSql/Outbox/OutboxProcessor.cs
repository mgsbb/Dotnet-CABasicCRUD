using System.Text.Json;
using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace CABasicCRUD.Infrastructure.Persistence.PostgreSql.Outbox;

public sealed class OutboxProcessor(
    IServiceProvider serviceProvider,
    ILogger<OutboxProcessor> logger
) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<OutboxProcessor> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessOutboxAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    private async Task ProcessOutboxAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var dispatcher = scope.ServiceProvider.GetRequiredService<IDomainEventDispatcher>();

        var jsonSerializerOptions =
            scope.ServiceProvider.GetRequiredService<JsonSerializerOptions>();

        var messages = await dbContext
            .Set<OutboxMessage>()
            .Where(x => x.ProcessedOnUtc == null)
            .OrderBy(x => x.OccurredOnUtc)
            .Take(10)
            .ToListAsync(cancellationToken);

        foreach (var message in messages)
        {
            AsyncRetryPolicy policy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (exception, delay, attemp, context) =>
                    {
                        _logger.LogWarning(
                            exception,
                            "Outbox retry {retryAttempt} after {delay}s",
                            attemp,
                            delay.TotalSeconds
                        );
                    }
                );

            PolicyResult result = await policy.ExecuteAndCaptureAsync(async () =>
            {
                var type = Type.GetType(message.Type)!;

                _logger.LogInformation("Dispatching domain event {@type}", type);

                var domainEvent = (IDomainEvent)
                    JsonSerializer.Deserialize(message.Content, type, jsonSerializerOptions)!;

                await dispatcher.DispatchAsync(domainEvent, cancellationToken);
            });

            message.Error = result.FinalException?.ToString();
            message.ProcessedOnUtc = DateTime.UtcNow;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
