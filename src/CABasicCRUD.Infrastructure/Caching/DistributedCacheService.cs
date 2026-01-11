using System.Collections.Concurrent;
using System.Text.Json;
using CABasicCRUD.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace CABasicCRUD.Infrastructure.Caching;

public sealed class DistributedCacheService(
    IDistributedCache distributedCache,
    JsonSerializerOptions jsonSerializerOptions
) : ICacheService
{
    private static readonly ConcurrentDictionary<string, bool> CacheKeys = new();
    private readonly IDistributedCache _distributedCache = distributedCache;
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions;

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        where T : class
    {
        string? cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);

        if (cachedValue is null)
            return null;

        T? value = JsonSerializer.Deserialize<T>(cachedValue, _jsonSerializerOptions);

        return value;
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        CancellationToken cancellationToken = default
    )
        where T : class
    {
        string cacheValue = JsonSerializer.Serialize(value, _jsonSerializerOptions);

        await _distributedCache.SetStringAsync(key, cacheValue, cancellationToken);

        CacheKeys.TryAdd(key, false);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);

        CacheKeys.TryRemove(key, out bool _);
    }

    public async Task RemoveByPrefixAsync(
        string prefixKey,
        CancellationToken cancellationToken = default
    )
    {
        IEnumerable<Task> tasks = CacheKeys
            .Keys.Where(k => k.StartsWith(prefixKey))
            .Select(k => RemoveAsync(k, cancellationToken));

        await Task.WhenAll(tasks);
    }
}
