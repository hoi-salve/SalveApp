using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SalveApp.Clinics.Core.Config;
using SalveApp.Clinics.Core.Models;
using SalveApp.Clinics.Core.Services;

namespace SalveApp.Clinics.Core.Cache;

public class CachedClinicsService : IClinicsService
{
    private readonly IDistributedCache _cache;
    private readonly IClinicsService _clinicsService;
    private readonly ILogger<CachedClinicsService> _logger;
    private readonly int _minsToCache;

    public CachedClinicsService(IClinicsService clinicsService,
        IDistributedCache cache, IOptionsMonitor<CacheServicesConfig> options,
        ILogger<CachedClinicsService> logger)
    {
        _clinicsService = clinicsService;
        _cache = cache;
        _logger = logger;
        _minsToCache = options.Get(CacheServicesConfig.CacheServices).MinsToCache;
    }

    public async Task<List<Clinic>> GetAllClinics()
    {
        return await GetCachedClinics(() => _clinicsService.GetAllClinics());
    }

    public async Task<List<Patient>> GetPatients(int clinicId)
    {
        return await GetCachedClinics(() => _clinicsService.GetPatients(clinicId), clinicId);
    }

    private async Task<List<TU>> GetCachedClinics<TU>(Func<Task<List<TU>>> getAll, int id = 0)
    {
        var cacheKey = $"current_clinics_{id}_{DateTime.UtcNow:yyyy_MM_dd}";

        var (isCached, clinics) = await _cache.TryGetValueAsync<List<TU>>(cacheKey);
        if (isCached)
        {
            _logger.LogDebug($"Cached version for {cacheKey} is found");
            return clinics;
        }

        _logger.LogDebug($"Creating a new cached version for {cacheKey}");

        var result = await getAll();

        if (result != null)
            await _cache.SetAsync(cacheKey, result, _minsToCache);

        return result;
    }
}