using System;
using Microsoft.Extensions.DependencyInjection;

namespace SalveApp.Clinics.Core.Cache;

public class DistributedCacheFactory : IDistributedCacheFactory
{
    private readonly IServiceProvider _serviceProvider;

    public DistributedCacheFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IDistributedCache GetCache<T>()
    {
        return _serviceProvider.GetRequiredService<IDistributedCache>();
    }
}