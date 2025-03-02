using CoinDesk.DTOs;
using CoinDesk.Repositories;
using CoinDesk.Resources;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Text.Json;

namespace CoinDesk.Services
{
    public class CoinDeskService : ICoinDeskService
    {
        private readonly ICoinDeskRepository _coinDeskRepository;

        private readonly ICurrencyRepository _currencyRepository;

        private readonly IStringLocalizer<SharedResource> _localizer;

        public CoinDeskService(ICoinDeskRepository coinDeskRepository, ICurrencyRepository currencyRepository, IStringLocalizer<SharedResource> localizer)
        {
            _coinDeskRepository = coinDeskRepository;
            _currencyRepository = currencyRepository;
            _localizer = localizer;
        }

        public async Task<RateResponse> GetBitcoinRatesAsync()
        {
            var bitcoinPrice = await GetBitcoinPriceAsync();
            var allCurrencies = await _currencyRepository.GetCurrenciesAsync();
            return new RateResponse
            {
                UpdatedAt = GetFormattedTime(bitcoinPrice.Time.UpdatedISO),
                Rates = bitcoinPrice.Bpi.Select(bpi => new CurrencyRate
                {
                    Code = bpi.Value.Code,
                    Name = _localizer[allCurrencies.FirstOrDefault(c => c.Code == bpi.Value.Code)?.Name ?? "Unknown"],
                    Rate = bpi.Value.RateFloat
                })
            };
        }

        private async Task<CoinDeskResponse> GetBitcoinPriceAsync()
        {
            try
            {
                return await _coinDeskRepository.GetBitcoinPriceAsync() ?? GetMockData();
            }
            catch (Exception ex)
            {
                return GetMockData();
            }
        }

        private CoinDeskResponse GetMockData()
        {
            var json = """
                {
                    "time": {
                        "updated": "Aug 3, 2022 20:25:00 UTC",
                        "updatedISO": "2022-08-03T20:25:00+00:00",
                        "updateduk": "Aug 3, 2022 at 21:25 BST"
                    },
                    "disclaimer": "This data was produced from the CoinDesk Bitcoin Price Index (USD). Non-USD currency data converted using hourly conversion rate from openexchangerates.org",
                    "chartName": "Bitcoin",
                    "bpi": {
                        "USD": {
                            "code": "USD",
                            "symbol": "$",
                            "rate": "23,342.0112",
                            "description": "US Dollar",
                            "rate_float": 23342.0112
                        },
                        "GBP": {
                            "code": "GBP",
                            "symbol": "£",
                            "rate": "19,504.3978",
                            "description": "British Pound Sterling",
                            "rate_float": 19504.3978
                        },
                        "EUR": {
                            "code": "EUR",
                            "symbol": "€",
                            "rate": "22,738.5269",
                            "description": "Euro",
                            "rate_float": 22738.5269
                        }
                    }
                }
                """;

            return JsonSerializer.Deserialize<CoinDeskResponse>(json);
        }

        private string GetFormattedTime(string updatedISO)
        {
            DateTime utcTime = DateTime.Parse(updatedISO, null, DateTimeStyles.AdjustToUniversal);
            TimeZoneInfo taipeiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
            DateTime taipeiTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, taipeiTimeZone);
            return taipeiTime.ToString("yyyy/MM/dd HH:mm:ss");
        }
    }
}