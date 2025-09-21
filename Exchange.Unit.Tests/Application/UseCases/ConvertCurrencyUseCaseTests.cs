using Exchange.Application.Dtos.Requests;
using Exchange.Application.UseCases.ConvertCurrency;
using Exchange.Domain.Entities;
using Exchange.Domain.Enums;
using Exchange.Domain.Interfaces;
using Moq;
using Xunit;

namespace Exchange.Unit.Tests.Application.UseCases
{
    public class ConvertCurrencyUseCaseTests
    {
        private readonly Mock<IExchangeRateProvider> _rateProviderMock;
        private readonly Mock<IConversionRepository> _repositoryMock;
        private readonly ConvertCurrencyUseCase _useCase;

        public ConvertCurrencyUseCaseTests()
        {
            _rateProviderMock = new Mock<IExchangeRateProvider>();
            _repositoryMock = new Mock<IConversionRepository>();
            _useCase = new ConvertCurrencyUseCase(_rateProviderMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldThrowArgumentException_WhenAmountBRLIsZero()
        {
            // Arrange
            var request = new ConvertCurrencyRequest(
                ToCurrency: "EUR",
                AmountBRL: 0,
                DateQuotation: DateOnly.FromDateTime(DateTime.Today),
                ExchangeType: ExchangeQuotationEnum.Buy
            );

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _useCase.ExecuteAsync(request));
        }

        [Fact]
        public async Task ExecuteAsync_ShouldConvertCurrency_WhenExchangeTypeIsBuy()
        {
            // Arrange
            var request = new ConvertCurrencyRequest(
                ToCurrency: "EUR",
                AmountBRL: 1000m,
                DateQuotation: DateOnly.FromDateTime(DateTime.Today),
                ExchangeType: ExchangeQuotationEnum.Buy
            );

            var exchangeRate = new ExchangeRate("EUR", 6.0m, 6.2m, "2025-08-13");

            _rateProviderMock
                .Setup(x => x.GetExchangeRateAsync(request.ToCurrency, request.DateQuotation))
                .ReturnsAsync(exchangeRate);

            // Act
            var response = await _useCase.ExecuteAsync(request);

            // Assert
            Assert.Equal(1000m, response.OriginalAmount);
            Assert.Equal("BRL", response.FromCurrency);
            Assert.Equal("EUR", response.ToCurrency);
            Assert.Equal(Math.Round(1000m / exchangeRate.BuyRate, 2), response.ConvertedAmount);
            Assert.Equal(Math.Round(exchangeRate.BuyRate, 2), response.ExchangeRate);

            _repositoryMock.Verify(r => r.SaveAsync(It.IsAny<ConversionRecord>()), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldConvertCurrency_WhenExchangeTypeIsSell()
        {
            // Arrange
            var request = new ConvertCurrencyRequest(
                ToCurrency: "EUR",
                AmountBRL: 1000m,
                DateQuotation: DateOnly.FromDateTime(DateTime.Today),
                ExchangeType: ExchangeQuotationEnum.Sell
            );

            var exchangeRate = new ExchangeRate("EUR", 6.0m, 6.2m, "2025-08-13");

            _rateProviderMock
                .Setup(x => x.GetExchangeRateAsync(request.ToCurrency, request.DateQuotation))
                .ReturnsAsync(exchangeRate);

            // Act
            var response = await _useCase.ExecuteAsync(request);

            // Assert
            Assert.Equal(Math.Round(1000m / exchangeRate.SellRate, 2), response.ConvertedAmount);
            Assert.Equal(Math.Round(exchangeRate.SellRate, 2), response.ExchangeRate);

            _repositoryMock.Verify(r => r.SaveAsync(It.IsAny<ConversionRecord>()), Times.Once);
        }
    }
}
