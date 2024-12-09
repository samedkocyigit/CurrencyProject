using BusinessAPI.Infrastructure;
using BusinessAPI.Models;
using System.Net.Http;
using System.Text.Json;

namespace BusinessAPI.Services
{
    public class CurrencyService
    {
        private readonly CacheDataSource _cacheDataSource;
        private readonly HttpClient _httpClient;
        public CurrencyService(CacheDataSource cacheDataSource, HttpClient httpClient)
        {
            _cacheDataSource = cacheDataSource;
            _httpClient = httpClient;
        }

        public async Task<List<CurrencyDTO>> GetCurrencyByCurrencyCodeAsync(string currencyCode)
        {
            var cachedData = await _cacheDataSource.GetCachedRatesByCurrencyCodeAsync(currencyCode);
            if (cachedData != null)
            {
                return cachedData;
            }

            var response = await _httpClient.GetAsync($"http://dataapi-container:80/api/Currency/{currencyCode}");

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new UtcDateTimeConverter() }
                };

                var currencies = await response.Content.ReadFromJsonAsync<List<CurrencyDTO>>(options);
                var twoMonthsAgo =  DateTime.Today.AddMonths(-2).ToUniversalTime();
                var filteredCurrencies = currencies.Where(x => x.Date >=  twoMonthsAgo.Date).ToList();

                return filteredCurrencies;
            }

            throw new Exception("Data API'den döviz kuru alınamadı.");
        }
        public async Task CacheAllCurrenciesAsync()
        {
            var response = await _httpClient.GetAsync($"http://dataapi-container:80/api/Currency/get-all-currencies");

            if (response.IsSuccessStatusCode)
            {
                var currencies = await response.Content.ReadFromJsonAsync<List<CurrencyDTO>>();

                if (currencies != null)
                {
                    foreach (var currency in currencies)
                    {
                        await _cacheDataSource.SetCacheRateAsync(currency);
                    }
                }
            }
            else
            {
                throw new Exception("Data API'den döviz kuru alınamadı.");
            }
        }
        public async Task<List<CurrencyDTO>> GetAllCurrenciesAsync() 
        {
            var allCachedData = await _cacheDataSource.GetAllCachedRatesAsync();

            if(allCachedData != null)
            {
                return allCachedData;
            }
            return new List<CurrencyDTO>();
        }


    }
}
