using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

public class CurrencyController : Controller
{
    private readonly HttpClient _httpClient;

    public CurrencyController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Index method to load the page
    public IActionResult Index()
    {
        return View();
    }

    // AJAX method to get the exchange rate from the Business API
    [HttpPost]
    public async Task<IActionResult> GetExchangeRate(string currencyCode)
    {
        var response = await _httpClient.GetAsync($"https://localhost:7048/api/exchange-rate/{currencyCode}");

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            return Json(data);
        }

        return Json(new { error = "Data not found" });
    }
}

