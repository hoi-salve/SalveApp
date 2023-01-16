using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SalveApp.Clinics.Core.Cache;

namespace SalveApp.Clinics.Web.IOC;

public static class CachingServiceCollectionExtensions
{
    public static IServiceCollection AddCaching(this IServiceCollection services)
    {
        services.AddDistributedMemoryCache();

        services.TryAddSingleton(typeof(IDistributedCache),
            typeof(DistributedCache)); // open generic registration

        services.TryAddSingleton<IDistributedCacheFactory, DistributedCacheFactory>();

        return services;
    }
}