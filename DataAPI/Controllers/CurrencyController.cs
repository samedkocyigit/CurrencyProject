using DataAPI.Models;
using DataAPI.Services.CurrencyServices;
using Microsoft.AspNetCore.Mvc;

namespace DataAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet("fetch-currencies-from-tcmb")]
        public async Task<IActionResult> FetchCurrencyRates()
        {
            DateTime startDate = DateTime.Today.AddMonths(-2).ToUniversalTime();
            DateTime endDate = DateTime.Today.ToUniversalTime();

            await _currencyService.FetchCurrencyRate(startDate, endDate);
            return Ok();
        }
        [HttpGet("get-all-currencies")]
        public async Task<IActionResult> FetchAllCurrencies()
        {
            var currencies = await _currencyService.GetAllCurrency();
            return Ok(currencies);
        }
        [HttpPost("create-currency")]
        public async Task<IActionResult> CreateCurrency(CurrencyRate currencyRate)
        {
            var currency = _currencyService.AddCurrency(currencyRate);
            return Ok(currency);
        }
        [HttpDelete("delete-currency/{id}")]
        public async Task<IActionResult> DeleteCurrency(int id)
        {
            await _currencyService.DeleteCurrency(id);
            return Ok();
        }
        [HttpGet("{currencyCode}")]
        public async Task<IActionResult> GetCurrencyWithCurrencyCode(string currencyCode)
        {
            var currencies = await _currencyService.GetCurrencyByCurrencyCode(currencyCode);
            return Ok(currencies);
        }

        //[HttpGet("fetch-all-currencies-with-cache")]
        //public async Task<IActionResult> FetchCurrenciesWithCache()
        //{
        //    var currencies = await _currencyService.GetAllCurrencyWithCache();
        //    return Ok(currencies);
        //}


    }
}
