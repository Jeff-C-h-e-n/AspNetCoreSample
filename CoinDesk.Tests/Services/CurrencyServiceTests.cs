using CoinDesk.Models;
using CoinDesk.Repositories;
using Moq;

namespace CoinDesk.Tests.Services

{
    public class CurrencyServiceTests
    {
        private readonly Mock<ICurrencyRepository> _mockRepo;
        private readonly CurrencyService _service;

        public CurrencyServiceTests()
        {
            _mockRepo = new Mock<ICurrencyRepository>();
            _service = new CurrencyService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetCurrencyByCodeAsync_ShouldReturnCurrency_WhenCurrencyExists()
        {
            // Arrange
            var currency = new Currency { Code = "TST", Name = "測試" };
            _mockRepo.Setup(r => r.GetCurrencyAsync(currency.Code)).ReturnsAsync(currency);

            // Act
            var result = await _service.GetCurrencyByCodeAsync(currency.Code);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(currency, result);
        }

        [Fact]
        public async Task GetCurrencyByCodeAsync_ShouldReturnNull_WhenCurrencyDoesNotExist()
        {
            // Arrange
            var currency = new Currency { Code = "TST", Name = "測試" };
            _mockRepo.Setup(r => r.GetCurrencyAsync(currency.Code)).ReturnsAsync((Currency)null);

            // Act
            var result = await _service.GetCurrencyByCodeAsync(currency.Code);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllCurrenciesOrderByCodeAsync_ShouldReturnOrderedList_WhenCurreniesExists()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetCurrenciesAsync()).ReturnsAsync(new List<Currency>
            {
                new Currency { Code = "USD", Name = "美金" },
                new Currency { Code = "EUR", Name = "歐元" }
            });

            // Act
            var result = await _service.GetAllCurrenciesOrderByCodeAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("EUR", result.First().Code);
            Assert.Equal("歐元", result.First().Name);
            Assert.Equal("USD", result.Last().Code);
            Assert.Equal("美金", result.Last().Name);
        }

        [Fact]
        public async Task GetAllCurrenciesOrderByCodeAsync_ShouldReturnEmptyList_WhenCurreniesDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetCurrenciesAsync()).ReturnsAsync(new List<Currency>());

            // Act
            var result = await _service.GetAllCurrenciesOrderByCodeAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task AddCurrencyAsync_ShouldReturnTrue_WhenCurrencyDoesNotExist()
        {
            // Arrange
            var currency = new Currency { Code = "TST", Name = "測試" };
            _mockRepo.Setup(r => r.GetCurrencyAsync(currency.Code)).ReturnsAsync((Currency)null);

            // Act
            var result = await _service.AddCurrencyAsync(currency);

            // Assert
            Assert.True(result);
            _mockRepo.Verify(r => r.AddCurrencyAsync(currency), Times.Once);
        }

        [Fact]
        public async Task AddCurrencyAsync_ShouldReturnFalse_WhenCurrencyAlreadyExists()
        {
            // Arrange
            var currency = new Currency { Code = "TST", Name = "測試" };
            _mockRepo.Setup(r => r.GetCurrencyAsync(currency.Code)).ReturnsAsync(currency);

            // Act
            var result = await _service.AddCurrencyAsync(currency);

            // Assert
            Assert.False(result);
            _mockRepo.Verify(r => r.AddCurrencyAsync(It.IsAny<Currency>()), Times.Never);
        }

        [Fact]
        public async Task UpdateCurrencyNameByCodeAsync_ShouldReturnTrue_WhenCurrencyExists()
        {
            // Arrange
            var currency = new Currency { Code = "TST", Name = "測試" };
            _mockRepo.Setup(r => r.GetCurrencyAsync(currency.Code)).ReturnsAsync(currency);

            // Act
            var result = await _service.UpdateCurrencyNameByCodeAsync(currency.Code, currency);

            // Assert
            Assert.True(result);
            _mockRepo.Verify(r => r.UpdateCurrencyNameAsync(currency, currency.Name), Times.Once);
        }

        [Fact]
        public async Task UpdateCurrencyNameByCodeAsync_ShouldReturnFalse_WhenCurrencyDoesNotExist()
        {
            // Arrange
            var currency = new Currency { Code = "TST", Name = "測試" };
            _mockRepo.Setup(r => r.GetCurrencyAsync(currency.Code)).ReturnsAsync((Currency)null);

            // Act
            var result = await _service.UpdateCurrencyNameByCodeAsync(currency.Code, currency);

            // Assert
            Assert.False(result);
            _mockRepo.Verify(r => r.UpdateCurrencyNameAsync(It.IsAny<Currency>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task DeleteCurrencyByCodeAsync_ShouldReturnTrue_WhenCurrencyExists()
        {
            // Arrange
            var currency = new Currency { Code = "TST", Name = "測試" };
            _mockRepo.Setup(r => r.GetCurrencyAsync(currency.Code)).ReturnsAsync(currency);

            // Act
            var result = await _service.DeleteCurrencyByCodeAsync(currency.Code);

            // Assert
            Assert.True(result);
            _mockRepo.Verify(r => r.DeleteCurrencyAsync(currency), Times.Once);
        }

        [Fact]
        public async Task DeleteCurrencyByCodeAsync_ShouldReturnFalse_WhenCurrencyDoesNotExist()
        {
            // Arrange
            var currency = new Currency { Code = "TST", Name = "測試" };
            _mockRepo.Setup(r => r.GetCurrencyAsync(currency.Code)).ReturnsAsync((Currency)null);

            // Act
            var result = await _service.DeleteCurrencyByCodeAsync(currency.Code);

            // Assert
            Assert.False(result);
            _mockRepo.Verify(r => r.DeleteCurrencyAsync(It.IsAny<Currency>()), Times.Never);
        }
    }
}