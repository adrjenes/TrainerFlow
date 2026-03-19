using FluentAssertions;
using Moq;
using TrainerFlow.Modules.Offers.Features.GetOfferBySlug;

namespace TrainerFlow.Tests.Unit.Modules.Offers.Features.GetOffersBySlug;

public sealed class GetOfferBySlugHandlerTests
{
    [Fact]
    public async Task HandleAsync_ShouldReturnOffer_WhenOfferExists()
    {
        // Arrange
        var repositoryMock = new Mock<IGetOfferBySlugReadRepository>();
        var expected = new OfferDetailsResponse(1, "plan-treningowy", "Plan treningowy", 199m);

        repositoryMock
            .Setup(x => x.GetBySlugAsync("plan-treningowy", It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var handler = new GetOfferBySlugHandler(repositoryMock.Object);
        var query = new GetOfferBySlugQuery("plan-treningowy");

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task HandleAsync_ShouldPassNormalizedSlugToRepository_WhenSlugContainsSpacesAndUppercase()
    {
        // Arrange
        var repositoryMock = new Mock<IGetOfferBySlugReadRepository>();

        repositoryMock
            .Setup(x => x.GetBySlugAsync("plan-treningowy", It.IsAny<CancellationToken>()))
            .ReturnsAsync((OfferDetailsResponse?)null);

        var handler = new GetOfferBySlugHandler(repositoryMock.Object);

        var query = new GetOfferBySlugQuery(" PLAN-TRENINGOWY ");

        // Act
        await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        repositoryMock.Verify(
            x => x.GetBySlugAsync("plan-treningowy", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnNull_WhenSlugIsEmpty()
    {
        // Arrange
        var repositoryMock = new Mock<IGetOfferBySlugReadRepository>();
        var handler = new GetOfferBySlugHandler(repositoryMock.Object);

        var query = new GetOfferBySlugQuery(" ");

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();

        repositoryMock.Verify(
            x => x.GetBySlugAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnNull_WhenOfferDoesNotExist()
    {
        // Arrange
        var repositoryMock = new Mock<IGetOfferBySlugReadRepository>();

        repositoryMock
            .Setup(x => x.GetBySlugAsync("unknown", It.IsAny<CancellationToken>()))
            .ReturnsAsync((OfferDetailsResponse?)null);

        var handler = new GetOfferBySlugHandler(repositoryMock.Object);

        var query = new GetOfferBySlugQuery("unknown");

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}
