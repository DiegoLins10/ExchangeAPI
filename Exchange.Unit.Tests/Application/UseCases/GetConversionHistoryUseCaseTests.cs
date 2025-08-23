using Exchange.Application.UseCases.GetConversionHistory;
using Exchange.Domain.Entities;
using Exchange.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace Exchange.Unit.Tests.Application.UseCases
{
    public class GetConversionHistoryUseCaseTests
    {
        private readonly Mock<IConversionRepository> _repositoryMock;
        private readonly GetConversionHistoryUseCase _useCase;
        private readonly IMemoryCache _memoryCacheMock;


        public GetConversionHistoryUseCaseTests()
        {
            _repositoryMock = new Mock<IConversionRepository>();
            _memoryCacheMock = new MemoryCache(new MemoryCacheOptions());

            // Instancia o use case com os dois mock
            _useCase = new GetConversionHistoryUseCase(_repositoryMock.Object, _memoryCacheMock);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnEmptyList_WhenNoHistoryExists()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetHistoryAsync())
                .ReturnsAsync(new List<ConversionRecord>());

            // Act
            var result = await _useCase.ExecuteAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnHistory_WhenRecordsExist()
        {
            // Arrange
            var records = new List<ConversionRecord>
            {
                new ConversionRecord("BRL", "EUR", 1000m, 166.67m, 6m),
                new ConversionRecord("BRL", "USD", 500m, 83.33m, 6m)
            };

            _repositoryMock.Setup(r => r.GetHistoryAsync())
                .ReturnsAsync(records);

            // Act
            var result = await _useCase.ExecuteAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, r => r.ToCurrency == "EUR" && r.OriginalAmount == 1000m);
            Assert.Contains(result, r => r.ToCurrency == "USD" && r.OriginalAmount == 500m);
        }
    }
}
