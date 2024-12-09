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

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet()]
    public async Task<IActionResult> GetExchangeRate(string currencyCode)
    {
        var response = await _httpClient.GetAsync($"http://businessapi-container:81/api/Currency/{currencyCode}");

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            return Json(data);
        }

        return Json(new { error = "Data not found" });
    }
}

