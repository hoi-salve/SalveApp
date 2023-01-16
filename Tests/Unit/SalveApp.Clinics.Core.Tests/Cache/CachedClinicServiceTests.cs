using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using SalveApp.Clinics.Core.Cache;
using SalveApp.Clinics.Core.Config;
using SalveApp.Clinics.Core.Models;
using SalveApp.Clinics.Core.Services;
using SalveApp.Clinics.Core.Tests.Cache.Helpers;
using Xunit;

namespace SalveApp.Clinics.Core.Tests.Cache;

public class CachedClinicServiceTests
{
    [Fact]
    public async Task When_cache_time_is_set_in_the_config_its_correctly_set_for_the_cached_service()
    {
        // Arrange
        const int expectedMinsToCache = 101;
        var minsToCache = -1;

        var ClinicsMock = new Mock<IClinicsService>();
        ClinicsMock.Setup(x => x.GetAllClinics())
            .ReturnsAsync(DummyData.Clinics().ToList);

        var cacheMock = new Mock<IDistributedCache>();
        cacheMock.Setup(x => x.TryGetValueAsync<List<Clinic>>(It.IsAny<string>()))
            .ReturnsAsync((false, (List<Clinic>)null));

        cacheMock.Setup(x => x
                .SetAsync(It.IsAny<string>(), It.IsAny<List<Clinic>>(), It.IsAny<int>()))
            .Callback<string, List<Clinic>, int>((key, result, mins) => minsToCache = mins);

        var optionsMock = new Mock<IOptionsMonitor<CacheServicesConfig>>();
        optionsMock.Setup(x => x.Get(CacheServicesConfig.CacheServices))
            .Returns(new CacheServicesConfig { MinsToCache = expectedMinsToCache });

        var sut = new CachedClinicsService(ClinicsMock.Object, cacheMock.Object, optionsMock.Object,
            NullLogger<CachedClinicsService>.Instance);


        // Act
        _ = await sut.GetAllClinics();

        // Assert
        cacheMock.Verify(x => x.SetAsync(
            It.IsAny<string>(), It.IsAny<List<Clinic>>(), It.IsAny<int>()), Times.Once);

        Assert.Equal(expectedMinsToCache, minsToCache);
    }


    [Fact]
    public async Task When_cache_time_is_set_in_the_config_its_correctly_set_for_the_cached_service_via_stub_services()
    {
        // Arrange
        const int expectedMinsToCache = 101;
        var stubCache = new StubDistributedCache();
        var options = new ServiceCollection()
            .Configure<CacheServicesConfig>(CacheServicesConfig.CacheServices, opt =>
                opt.MinsToCache = expectedMinsToCache)
            .BuildServiceProvider()
            .GetRequiredService<IOptionsMonitor<CacheServicesConfig>>();

        var sut = new CachedClinicsService(new StubClinicService(), stubCache, options,
            NullLogger<CachedClinicsService>.Instance);

        // Act
        _ = await sut.GetAllClinics();

        // Assert
        Assert.True(stubCache.ItemCached);
        Assert.Equal(expectedMinsToCache, stubCache.CachedForMins);
    }
}