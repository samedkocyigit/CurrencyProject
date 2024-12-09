using DataAPI.Infrastructure.GenericRepository;
using DataAPI.Models;

namespace DataAPI.Infrastructure.CurrencyRepository
{
    public interface ICurrencyRepository : IGenericRepository<CurrencyRate>
    {
        Task<IEnumerable<CurrencyRate>> GetCurrencies();
        Task<IEnumerable<CurrencyRate>> GetCurrencyByCurrencyCode(string currencyCode);
        Task<CurrencyRate> GetCurrencyByCurrencyCodeAndDate(string currencyCode, DateTime date);
        Task RemoveCurrenciesBeforeTwoMonth();
    }
}
