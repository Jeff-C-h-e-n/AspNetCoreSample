namespace CoinDesk.DTOs
{
    public class RateResponse
    {
        public string UpdatedAt { get; set; }

        public IEnumerable<CurrencyRate> Rates { get; set; }
    }

    public class CurrencyRate
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public decimal Rate { get; set; }
    }
}