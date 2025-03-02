using CoinDesk.Controllers;
using CoinDesk.DTOs;
using CoinDesk.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CoinDesk.Tests.Controllers
{
    public class CoinDeskControllerTests
    {
        private readonly Mock<ICoinDeskService> _mockService;
        private readonly CoinDeskController _controller;

        public CoinDeskControllerTests()
        {
            _mockService = new Mock<ICoinDeskService>();
            _controller = new CoinDeskController(_mockService.Object);
        }

        [Fact]
        public async Task GetBitcoinRates_ShouldReturnOk_WhenRatesExist()
        {
            // Arrange
            var mockRates = new RateResponse()
            {
                UpdatedAt = "2025/05/02 20:25:00",
                Rates = new List<CurrencyRate>
                {
                    new CurrencyRate { Code = "TST", Name = "測試", Rate = 12345.6789M }
                }
            };
            _mockService.Setup(s => s.GetBitcoinRatesAsync()).ReturnsAsync(mockRates);

            // Act
            var result = await _controller.GetBitcoinRates();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(mockRates, okResult.Value);
        }
    }
}