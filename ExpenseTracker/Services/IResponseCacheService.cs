using System;
using System.Threading.Tasks;

namespace ExpenseTracker.Services
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string cacheKey, object reponse, TimeSpan timeTimeLive);

        Task<string> GetCacheResponseAsync(string cacheKey);
    }
}