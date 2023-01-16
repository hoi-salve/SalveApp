using System;
using System.Threading.Tasks;
using SalveApp.Clinics.Core.Cache;

namespace SalveApp.Clinics.Core.Tests.Cache.Helpers;

public class StubDistributedCache : IDistributedCache
{
    public bool ItemCached { get; private set; }
    public int CachedForMins { get; private set; }

    public Task<T> GetAsync<T>(string key)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(string key)
    {
        throw new NotImplementedException();
    }

    public Task SetAsync<T>(string key, T item, int minutesToCache)
    {
        ItemCached = true;
        CachedForMins = minutesToCache;
        return Task.CompletedTask;
    }

    public Task<(bool Found, T Value)> TryGetValueAsync<T>(string key)
    {
        return Task.FromResult((false, default(T)));
    }
}