using DataAPI.Infrastructure.CurrencyRepository;
using DataAPI.Models;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace DataAPI.Services.CurrencyServices
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyService(ICurrencyRepository currencyRepository )
        {
            _currencyRepository = currencyRepository;
        }
        public async Task<IEnumerable<CurrencyRate>> GetAllCurrency()
        {
            var currencies = await _currencyRepository.GetAll();
            return currencies;

        }
        public async Task<CurrencyRate> GetCurrencyById(int id)
        {
            var currency = await _currencyRepository.GetById(id);
            return currency;
        }
        public async Task<IEnumerable<CurrencyRate>> GetCurrencyByCurrencyCode(string currencyCode)
        {
            var currencies = await _currencyRepository.GetCurrencyByCurrencyCode(currencyCode);
            return currencies;
        }
        public async Task<CurrencyRate> AddCurrency(CurrencyRate currencyRate)
        {
            var addedCurrency = await _currencyRepository.Add(currencyRate);
            return addedCurrency;
        }
        public async Task<CurrencyRate> UpdateCurrency(CurrencyRate currencyRate)
        {
            var existingCurrency = await _currencyRepository.GetById(currencyRate.Id);
            await _currencyRepository.Update(existingCurrency);
            return existingCurrency;
        }
        public async Task FetchCurrencyRate(DateTime startDate, DateTime endDate)
        {
            DateTime currentDate = startDate;
            var allRates = new List<CurrencyRate>();

            while (currentDate < endDate)
            {
                var rates = await FetchCurrencyRatesByDate(currentDate);
                foreach (var rate in rates)
                {
                    var existingRate = await _currencyRepository.GetCurrencyByCurrencyCodeAndDate(rate.CurrencyCode, rate.Date);
                    if (existingRate == null)
                        await _currencyRepository.Add(rate);
                    else
                    {
                        existingRate.ForexBuying = rate.ForexBuying;
                        existingRate.ForexSelling = rate.ForexSelling;
                        existingRate.BanknoteBuying = rate.BanknoteBuying;
                        existingRate.BanknoteSelling = rate.BanknoteSelling;
                        _currencyRepository.Update(existingRate);
                    }
                    allRates.Add(rate);
                }
                currentDate = currentDate.AddDays(1);
            }
        }
        private async Task<List<CurrencyRate>> FetchCurrencyRatesByDate(DateTime date)
        {
            var url = $"https://www.tcmb.gov.tr/kurlar/{date:yyyyMM}/{date:ddMMyyyy}.xml";
            List<CurrencyRate> currencyRates = new List<CurrencyRate>();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetStringAsync(url);
                    XDocument doc = XDocument.Parse(response);

                    var currencyElements = doc.Descendants("Currency");
                    foreach (var element in currencyElements)
                    {
                        var rate = new CurrencyRate
                        {
                            CurrencyCode = element.Attribute("CurrencyCode")?.Value,
                            Currency = element.Element("CurrencyName")?.Value ?? "Unknown",
                            Unit = int.Parse(element.Element("Unit")?.Value ?? "1"),
                            ForexBuying = decimal.Parse(element.Element("ForexBuying")?.Value ?? "0"),
                            ForexSelling = decimal.Parse(element.Element("ForexSelling")?.Value ?? "0"),
                            BanknoteBuying = decimal.Parse(element.Element("BanknoteBuying")?.Value ?? "0"),
                            BanknoteSelling = decimal.Parse(element.Element("BanknoteSelling")?.Value ?? "0"),
                            Date = date
                        };

                        currencyRates.Add(rate);
                    }
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine($"Data for {date.ToShortDateString()} could not be retrieved.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            return currencyRates;
        }
        public async Task RemoveCurrenciesBeforeTwoMonth()
        {
            await _currencyRepository.RemoveCurrenciesBeforeTwoMonth();
        }
        public async Task DeleteCurrency(int id)
        {
            await _currencyRepository.Delete(id);
        }
    }
}
