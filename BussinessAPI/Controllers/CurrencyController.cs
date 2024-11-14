using BussinessAPI.Models;
using BussinessAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BussinessAPI.Controllers
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
    }
}
