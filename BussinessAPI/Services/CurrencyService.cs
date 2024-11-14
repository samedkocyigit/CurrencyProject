using BusinessAPI.Services;
using BussinessAPI.Models;

namespace BussinessAPI.Services
{
    public class CurrencyService
    {
        private readonly CacheService _cacheService;
        private readonly HttpClient _httpClient;
        public CurrencyService(CacheService cacheService,HttpClient httpClient)
        {
            _cacheService = cacheService;
            _httpClient = httpClient;
        }

        public async Task<CurrencyDTO> GetCurrencyByCurrencyCodeAsync(string currencyCode)
        {
            var cachedRate = await _cacheService.GetCachedRateAsync(currencyCode);
            if (cachedRate != null)
            {
                return cachedRate;
            }

            var response = await _httpClient.GetAsync($"http://dataapi-url/api/exchangeRate/{currencyCode}");
            if (response.IsSuccessStatusCode)
            {
                var rate = await response.Content.ReadFromJsonAsync<CurrencyDTO>();

                await _cacheService.SetCacheRateAsync(currencyCode, rate);

                return rate;
            }

            throw new Exception("Data API'den döviz kuru alınamadı.");
        }
    }
}
