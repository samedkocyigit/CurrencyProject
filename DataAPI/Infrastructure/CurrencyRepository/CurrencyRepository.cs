using DataAPI.Infrastructure.GenericRepository;
using DataAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAPI.Infrastructure.CurrencyRepository
{
    public class CurrencyRepository : GenericRepository<CurrencyRate>, ICurrencyRepository
    {
        private readonly DotnetCurrencyProjectContext _context;

        public CurrencyRepository(DotnetCurrencyProjectContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CurrencyRate>> GetCurrencyByCurrencyCode(string currencyCode)
        {
            var currencies = await _context.CurrencyRates.Where(t => t.CurrencyCode == currencyCode).ToListAsync();
            if (currencies.Count == 0)
                throw new Exception("There is no Currency with that CurrencyCode");

            return currencies;
        }
        public async Task<CurrencyRate> GetCurrencyByCurrencyCodeAndDate(string currencyCode, DateTime date)
        {
            return await _context.CurrencyRates
                .FirstOrDefaultAsync(x => x.CurrencyCode == currencyCode && x.Date.Date == date.Date);

        }
    }
}
