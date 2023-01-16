namespace SalveApp.Clinics.Core.Cache;

public interface IDistributedCacheFactory
{
    IDistributedCache GetCache<T>();
}