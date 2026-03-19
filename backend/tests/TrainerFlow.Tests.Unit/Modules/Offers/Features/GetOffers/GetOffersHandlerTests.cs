using FluentAssertions;
using Moq;
using TrainerFlow.Modules.Offers.Features.GetOffers;

namespace TrainerFlow.Tests.Unit.Modules.Offers.Features.GetOffers;

public sealed class GetOffersHandlerTests
{
    [Fact]
    public async Task HandleAsync_ShouldReturnOffers_WhenOffersExist()
    {
        // Arrange
        var repositoryMock = new Mock<IGetOffersReadRepository>();

        IReadOnlyList<OfferResponse> expected =
        [
            new(1, "plan-treningowy", "Plan treningowy", 199m),
            new(2, "plan-dietetyczny", "Plan dietetyczny", 149m)
        ];

        repositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var handler = new GetOffersHandler(repositoryMock.Object);
        var query = new GetOffersQuery();

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnEmptyList_WhenNoOffersExist()
    {
        // Arrange
        var repositoryMock = new Mock<IGetOffersReadRepository>();

        IReadOnlyList<OfferResponse> expected = [];

        repositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var handler = new GetOffersHandler(repositoryMock.Object);
        var query = new GetOffersQuery();

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
}
