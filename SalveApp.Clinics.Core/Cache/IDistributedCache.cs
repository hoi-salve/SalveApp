using System.Threading.Tasks;

namespace SalveApp.Clinics.Core.Cache;

public interface IDistributedCache
{
    Task<T> GetAsync<T>(string key);
    Task RemoveAsync(string key);
    Task SetAsync<T>(string key, T item, int minutesToCache);
    Task<(bool Found, T Value)> TryGetValueAsync<T>(string key);
}