using System.Text.Json;
using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CABasicCRUD.Infrastructure.Persistence.Sqlite.Outbox;

public sealed class OutboxProcessor(IServiceProvider serviceProvider) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

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
            try
            {
                var type = Type.GetType(message.Type)!;

                var domainEvent = (IDomainEvent)
                    JsonSerializer.Deserialize(message.Content, type, jsonSerializerOptions)!;

                await dispatcher.DispatchAsync(domainEvent, cancellationToken);

                message.ProcessedOnUtc = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                message.Error = ex.ToString();
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
