namespace DataAPI.Models
{
    public class CurrencyRate
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string CurrencyCode { get; set; }
        public int Unit { get; set; }
        public string Currency { get; set; }
        public decimal ForexBuying { get; set; }
        public decimal ForexSelling { get; set; }
        public decimal BanknoteBuying { get; set; }
        public decimal BanknoteSelling { get; set; }
    }
}
