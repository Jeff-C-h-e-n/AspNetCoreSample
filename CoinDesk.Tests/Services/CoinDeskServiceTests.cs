using CoinDesk.DTOs;
using CoinDesk.Models;
using CoinDesk.Repositories;
using CoinDesk.Resources;
using CoinDesk.Services;
using Microsoft.Extensions.Localization;
using Moq;

namespace CoinDesk.Tests.Services
{
    public class CoinDeskServiceTests
    {
        private readonly Mock<ICoinDeskRepository> _mockCoinDeskRepo;
        private readonly Mock<ICurrencyRepository> _mockCurrencyRepo;
        private readonly Mock<IStringLocalizer<SharedResource>> _mockLocalizer;

        private readonly CoinDeskService _service;

        public CoinDeskServiceTests()
        {
            _mockCoinDeskRepo = new Mock<ICoinDeskRepository>();
            _mockCurrencyRepo = new Mock<ICurrencyRepository>();
            _mockLocalizer = new Mock<IStringLocalizer<SharedResource>>();
            _service = new CoinDeskService(_mockCoinDeskRepo.Object, _mockCurrencyRepo.Object, _mockLocalizer.Object);
        }

        [Fact]
        public async Task GetBitcoinRatesAsync_ShouldReturnRates_WhenApiCallSucceeds()
        {
            // Arrange
            _mockCoinDeskRepo.Setup(r => r.GetBitcoinPriceAsync()).ReturnsAsync(new CoinDeskResponse
            {
                Time = new TimeInfo { UpdatedISO = "2025-05-02T12:25:00+00:00" },
                Bpi = new Dictionary<string, CurrencyInfo>
                {
                    { "TST", new CurrencyInfo { Code = "TST", RateFloat = 12345.6789M } }
                }
            });

            _mockCurrencyRepo.Setup(r => r.GetCurrenciesAsync()).ReturnsAsync(new List<Currency>
            {
                new Currency { Code = "TST", Name = "測試" }
            });

            // Act
            var result = await _service.GetBitcoinRatesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("2025/05/02 20:25:00", result.UpdatedAt);
            Assert.NotEmpty(result.Rates);
            Assert.Equal("TST", result.Rates.First().Code);
            Assert.Equal(12345.6789M, result.Rates.First().Rate);
        }

        [Fact]
        public async Task GetBitcoinRatesAsync_ShouldReturnMockData_WhenApiCallFails()
        {
            // Arrange
            _mockCoinDeskRepo.Setup(r => r.GetBitcoinPriceAsync()).ThrowsAsync(new Exception());
            _mockCurrencyRepo.Setup(r => r.GetCurrenciesAsync()).ReturnsAsync(new List<Currency>
            {
                new Currency { Code = "USD", Name = "美金" }
            });

            // Act
            var result = await _service.GetBitcoinRatesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("2022/08/04 04:25:00", result.UpdatedAt);
            Assert.NotEmpty(result.Rates);
            Assert.Equal("USD", result.Rates.First().Code);
            Assert.Equal(23342.0112M, result.Rates.First().Rate);
        }
    }
}