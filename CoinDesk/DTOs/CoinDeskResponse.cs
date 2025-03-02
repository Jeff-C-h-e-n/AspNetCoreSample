using System.Text.Json.Serialization;

namespace CoinDesk.DTOs
{
    public class CoinDeskResponse
    {
        [JsonPropertyName("time")]
        public TimeInfo Time { get; set; }

        [JsonPropertyName("disclaimer")]
        public string Disclaimer { get; set; }

        [JsonPropertyName("chartName")]
        public string ChartName { get; set; }

        [JsonPropertyName("bpi")]
        public Dictionary<string, CurrencyInfo> Bpi { get; set; }
    }

    public class TimeInfo
    {
        [JsonPropertyName("updated")]
        public string Updated { get; set; }

        [JsonPropertyName("updatedISO")]
        public string UpdatedISO { get; set; }

        [JsonPropertyName("updateduk")]
        public string UpdatedUK { get; set; }
    }

    public class CurrencyInfo
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("rate")]
        public string Rate { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("rate_float")]
        public decimal RateFloat { get; set; }
    }
}