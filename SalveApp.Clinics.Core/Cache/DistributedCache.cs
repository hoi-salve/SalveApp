using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SalveApp.Clinics.Core.Cache;

public class DistributedCache : IDistributedCache
{
    private readonly string _cacheKeyPrefix;
    private readonly Microsoft.Extensions.Caching.Distributed.IDistributedCache _distributedCache;
    private readonly ILogger<DistributedCache> _logger;

    public DistributedCache(Microsoft.Extensions.Caching.Distributed.IDistributedCache distributedCache,
        ILogger<DistributedCache> logger)
    {
        _distributedCache = distributedCache;
        _logger = logger;

        _cacheKeyPrefix = $"{typeof(DistributedCache).Namespace}_{nameof(DistributedCache)}_";
    }

    public async Task<(bool Found, T Value)> TryGetValueAsync<T>(string key)
    {
        var value = await GetAsync<T>(key);

        return (value != null, value);
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var cachedResult = await _distributedCache.GetStringAsync(CacheKey(key));

        return cachedResult == null ? default : DeserialiseFromString<T>(cachedResult);
    }

    public async Task SetAsync<T>(string key, T item, int minutesToCache)
    {
        var cacheEntryOptions = new DistributedCacheEntryOptions
            { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(minutesToCache) };

        var serialisedItemToCache = SerialiseForCaching(item);

        await _distributedCache.SetStringAsync(CacheKey(key), serialisedItemToCache, cacheEntryOptions);
    }

    public Task RemoveAsync(string key)
    {
        return _distributedCache.RemoveAsync(CacheKey(key));
    }

    private string CacheKey(string key)
    {
        key = Regex.Replace(key, @"\s", "");
        return $"{_cacheKeyPrefix}{key.ToLower()}";
    }

    private T DeserialiseFromString<T>(string cachedResult)
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(cachedResult, new JsonSerializerSettings
            {
                MaxDepth = 10
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to deserialise from cached string");
            return default;
        }
    }

    private string SerialiseForCaching<T>(T item)
    {
        if (item == null) return null;

        try
        {
            return JsonConvert.SerializeObject(item);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to serialise type '{Type}' for caching", typeof(T).FullName);
            throw;
        }
    }
}