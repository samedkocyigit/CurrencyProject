
using System;
using System.Threading.Tasks;
using StackExchange.Redis;
using Newtonsoft.Json;
using BusinessAPI.Models;

namespace BusinessAPI.Infrastructure
{
    public class CacheDataSource
    {
        private readonly IDatabase _cacheDb;

        public CacheDataSource(IConnectionMultiplexer redis)
        {
            _cacheDb = redis.GetDatabase();
        }

        public async Task<List<CurrencyDTO>> GetCachedRatesByCurrencyCodeAsync(string currencyCode)
        {
            var currencies = new List<CurrencyDTO>();
            var twoMonthsAgo = DateTime.Today.AddMonths(-2);
            var hashKeys = await _cacheDb.HashKeysAsync(currencyCode);
            foreach (var hashKey in hashKeys)
            {
                var cachedData = await _cacheDb.HashGetAsync(currencyCode, hashKey);
                if (!cachedData.IsNullOrEmpty)
                {
                    var currency = JsonConvert.DeserializeObject<CurrencyDTO>(cachedData);

                    if (currency != null)
                    {
                        DateTime fieldDate = DateTime.Parse(hashKey); 

                        if (fieldDate >= twoMonthsAgo)
                        {
                            currencies.Add(currency);
                        }
                    }
                }
            }

            return currencies;
        }


        public async Task SetCacheRateAsync(CurrencyDTO currency)
        {
            var key = currency.CurrencyCode;
            var field = currency.Date.ToString("yyyy-MM-dd");

            var currencyJson = JsonConvert.SerializeObject(currency);

            await _cacheDb.HashSetAsync(key, field, currencyJson);
            await _cacheDb.SetAddAsync("allCurrencyCodes", currency.CurrencyCode);

        }

        public async Task<List<CurrencyDTO>> GetAllCachedRatesAsync()
        {
            var allCurrencies = new List<CurrencyDTO>();

            var currencyCodes = await _cacheDb.SetMembersAsync("allCurrencyCodes"); 
             
            foreach (var code in currencyCodes)
            {
                string currencyCode = code.ToString(); 

                var hashKeys = await _cacheDb.HashKeysAsync(currencyCode); 
                foreach (var hashKey in hashKeys)
                {
                    var cachedData = await _cacheDb.HashGetAsync(currencyCode, hashKey);
                    if (!cachedData.IsNullOrEmpty)
                    {
                        var currency = JsonConvert.DeserializeObject<CurrencyDTO>(cachedData);
                        if (currency != null)
                        {
                            allCurrencies.Add(currency);
                        }
                    }
                }
            }

            return allCurrencies;
        }

    }
}
