using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SalveApp.Clinics.Core.Cache;
using SalveApp.Clinics.Core.Services;

namespace SalveApp.Clinics.Web.IOC;

public static class SalveAppServiceCollectionExtensions
{
    public static IServiceCollection AddService(this IServiceCollection services, IConfiguration config)
    {
        services.TryAddSingleton<ICSVDataLoader, CSVDataLoader>();
        services.TryAddSingleton<IClinicsService, ClinicsService>();
        services.Decorate<IClinicsService, CachedClinicsService>();

        return services;
    }
}