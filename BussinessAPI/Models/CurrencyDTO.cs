namespace BussinessAPI.Models
{
    public class CurrencyDTO
    {
        public string CurrencyCode { get; set; } 
        public decimal ForexBuying { get; set; }
        public decimal BanknoteBuying { get; set; }
        public DateTime Date { get; set; }

    }
}
