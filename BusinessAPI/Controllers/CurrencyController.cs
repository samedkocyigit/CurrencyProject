using BusinessAPI.Models;
using BusinessAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BusinessAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly CurrencyService _currencyService;
        private readonly ILogger<CurrencyController> _logger;

        public CurrencyController(CurrencyService exchangeRateService, ILogger<CurrencyController> logger)
        {
            _currencyService = exchangeRateService;
            _logger = logger;
        }

        
        [HttpGet("{currencyCode}")]
        public async Task<ActionResult<CurrencyDTO>> GetExchangeRate(string currencyCode)
        {
            try
            {
                var rate = await _currencyService.GetCurrencyByCurrencyCodeAsync(currencyCode);
                if (rate == null)
                {
                    return NotFound($"There is no currencies with that '{currencyCode}' code.");
                }
                return Ok(rate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There is an error occured when fetchin currencies.");
                return StatusCode(500, "An error occurred. Please try again.");
            }
        }
        [HttpPost("cache-all-currencies")]
        public async Task<IActionResult> CacheAllCurrencies()
        {
            try
            {
                await _currencyService.CacheAllCurrenciesAsync();
                return Ok("All currencies have been cached successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GetAllCachedCurrencies")]
        public async Task<IActionResult> GetCurrencies() 
        {
            var currencies = await _currencyService.GetAllCurrenciesAsync();
            return Ok(currencies);
        }
    }
}
