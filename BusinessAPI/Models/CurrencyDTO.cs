using System.Text.Json.Serialization;

namespace BusinessAPI.Models
{
    public class CurrencyDTO
    {
        [JsonPropertyName("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonPropertyName("forexBuying")]
        public decimal ForexBuying { get; set; }

        [JsonConverter(typeof(UtcDateTimeConverter))]
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
    }

}
