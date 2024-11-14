using System;
using System.Threading.Tasks;
using StackExchange.Redis;
using Newtonsoft.Json;
using BussinessAPI.Models;

namespace BusinessAPI.Services
{
    public class CacheService
    {
        private readonly IDatabase _cacheDb;

        public CacheService(IConnectionMultiplexer redis)
        {
            _cacheDb = redis.GetDatabase();
        }

        public async Task<CurrencyDTO> GetCachedRateAsync(string currencyCode)
        {
            var cachedData = await _cacheDb.StringGetAsync(currencyCode);
            if (!cachedData.IsNullOrEmpty)
            {
                return JsonConvert.DeserializeObject<CurrencyDTO>(cachedData);
            }

            return null;
        }

        public async Task SetCacheRateAsync(string currencyCode, CurrencyDTO rate)
        {
            var serializedData = JsonConvert.SerializeObject(rate);
            await _cacheDb.StringSetAsync(currencyCode, serializedData, TimeSpan.FromHours(1));
        }
    }
}
