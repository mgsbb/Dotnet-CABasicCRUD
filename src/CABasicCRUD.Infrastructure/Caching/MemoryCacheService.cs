using System.Collections.Concurrent;
using CABasicCRUD.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace CABasicCRUD.Infrastructure.Caching;

public sealed class MemoryCacheService(IMemoryCache memoryCache) : ICacheService
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    private static readonly ConcurrentDictionary<string, bool> CacheKeys = new();

    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        where T : class
    {
        T? cachedValue = _memoryCache.TryGetValue(key, out T? value) ? value : default;
        return Task.FromResult(cachedValue);
    }

    public Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default)
        where T : class
    {
        _memoryCache.Set(key, value);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        _memoryCache.Remove(key);
        return Task.CompletedTask;
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
