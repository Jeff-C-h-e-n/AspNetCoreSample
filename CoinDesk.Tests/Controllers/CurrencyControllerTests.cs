using CoinDesk.Controllers;
using CoinDesk.Models;
using CoinDesk.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CoinDesk.Tests.Controllers
{
    public class CurrencyControllerTests
    {
        private readonly Mock<ICurrencyService> _mockService;
        private readonly CurrencyController _controller;

        public CurrencyControllerTests()
        {
            _mockService = new Mock<ICurrencyService>();
            _controller = new CurrencyController(_mockService.Object);
        }

        [Fact]
        public async Task GetCurrency_ShouldReturnNotFound_WhenCurrencyExist()
        {
            // Arrange
            var currency = new Currency { Code = "TST", Name = "測試" };
            _mockService.Setup(s => s.GetCurrencyByCodeAsync(currency.Code)).ReturnsAsync(currency);

            // Act
            var result = await _controller.GetCurrency(currency.Code);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(currency, okResult.Value);
        }

        [Fact]
        public async Task GetCurrency_ShouldReturnNotFound_WhenCurrencyDoesNotExist()
        {
            // Arrange
            var currency = new Currency { Code = "TST", Name = "測試" };
            _mockService.Setup(s => s.GetCurrencyByCodeAsync(currency.Code)).ReturnsAsync((Currency)null);

            // Act
            var result = await _controller.GetCurrency(currency.Code);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetCurrencies_ShouldReturnOk_WhenCurrenciesExist()
        {
            // Arrange
            var currencies = new List<Currency> { new Currency { Code = "TST", Name = "測試" } };
            _mockService.Setup(s => s.GetAllCurrenciesOrderByCodeAsync()).ReturnsAsync(currencies);

            // Act
            var result = await _controller.GetCurrencies();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(currencies, okResult.Value);
        }

        [Fact]
        public async Task GetCurrencies_ShouldReturnOk_WhenCurrenciesDoesNotExist()
        {
            // Arrange
            var currencies = new List<Currency> { };
            _mockService.Setup(s => s.GetAllCurrenciesOrderByCodeAsync()).ReturnsAsync(currencies);

            // Act
            var result = await _controller.GetCurrencies();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(currencies, okResult.Value);
        }

        [Fact]
        public async Task AddCurrency_ShouldReturnCreated_WhenCurrencyAddedSuccessfully()
        {
            // Arrange
            var currency = new Currency { Code = "TST", Name = "測試" };
            _mockService.Setup(s => s.AddCurrencyAsync(currency)).ReturnsAsync(true);

            // Act
            var result = await _controller.AddCurrency(currency);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
            Assert.Equal(currency, createdResult.Value);
        }

        [Fact]
        public async Task AddCurrency_ShouldReturnConflict_WhenCurrencyAlreadyExists()
        {
            // Arrange
            var currency = new Currency { Code = "TST", Name = "測試" };
            _mockService.Setup(s => s.AddCurrencyAsync(currency)).ReturnsAsync(false);

            // Act
            var result = await _controller.AddCurrency(currency);

            // Assert
            var conflictResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(409, conflictResult.StatusCode);
        }

        [Fact]
        public async Task UpdateCurrency_ShouldReturnNoContent_WhenUpdateSuccessful()
        {
            // Arrange
            var currency = new Currency { Code = "TST", Name = "測試" };
            _mockService.Setup(s => s.UpdateCurrencyNameByCodeAsync(currency.Code, currency)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateCurrency(currency.Code, currency);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateCurrency_ShouldReturnNotFound_WhenCurrencyDoesNotExist()
        {
            // Arrange
            var currency = new Currency { Code = "TST", Name = "測試" };
            _mockService.Setup(s => s.UpdateCurrencyNameByCodeAsync(currency.Code, currency)).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateCurrency(currency.Code, currency);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteCurrency_ShouldReturnNoContent_WhenDeleteSuccessful()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteCurrencyByCodeAsync("TST")).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteCurrency("TST");

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCurrency_ShouldReturnNotFound_WhenCurrencyDoesNotExist()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteCurrencyByCodeAsync("TST")).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteCurrency("TST");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}