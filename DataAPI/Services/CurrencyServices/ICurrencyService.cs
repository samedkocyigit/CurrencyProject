using DataAPI.Models;

namespace DataAPI.Services.CurrencyServices
{
    public interface ICurrencyService
    {
        Task<IEnumerable<CurrencyRate>> GetAllCurrency();
        Task<IEnumerable<CurrencyRate>> GetCurrencyByCurrencyCode(string currencyCode);
        Task<CurrencyRate> GetCurrencyById(int currencyId);
        Task<CurrencyRate> AddCurrency(CurrencyRate currencyRate);
        Task<CurrencyRate> UpdateCurrency(CurrencyRate currencyRate);
        Task DeleteCurrency(int currencyId);
        Task FetchCurrencyRate(DateTime statDate, DateTime endDate);

    }
}
