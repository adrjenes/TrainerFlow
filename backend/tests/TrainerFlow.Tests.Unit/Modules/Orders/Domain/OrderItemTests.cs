using FluentAssertions;
using TrainerFlow.Modules.Orders.Domain;

namespace TrainerFlow.Tests.Unit.Modules.Orders.Domain;

public sealed class OrderItemTests
{
    [Fact]
    public void Constructor_ShouldCreateOrderItem_WhenDataIsValid()
    {
        var orderId = Guid.NewGuid();

        var item = new OrderItem(orderId, 1, "Plan treningowy", 199m, 2);

        item.OrderId.Should().Be(orderId);
        item.OfferId.Should().Be(1);
        item.OfferNameSnapshot.Should().Be("Plan treningowy");
        item.Price.Should().Be(199m);
        item.Quantity.Should().Be(2);
    }

    [Fact]
    public void Constructor_ShouldTrimOfferNameSnapshot_WhenValueContainsSpaces()
    {
        var item = new OrderItem(Guid.NewGuid(), 1, "  Plan treningowy  ", 199m, 1);

        item.OfferNameSnapshot.Should().Be("Plan treningowy");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenOrderIdIsEmpty()
    {
        Action act = () => new OrderItem(Guid.Empty, 1, "Plan treningowy", 199m, 1);

        act.Should().Throw<ArgumentException>()
            .WithParameterName("orderId");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenOfferIdIsNotGreaterThanZero(int offerId)
    {
        Action act = () => new OrderItem(Guid.NewGuid(), offerId, "Plan treningowy", 199m, 1);

        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("offerId");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenOfferNameSnapshotIsEmpty()
    {
        Action act = () => new OrderItem(Guid.NewGuid(), 1, " ", 199m, 1);

        act.Should().Throw<ArgumentException>()
            .WithParameterName("offerNameSnapshot");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-0.01)]
    public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenPriceIsNegative(decimal price)
    {
        Action act = () => new OrderItem(Guid.NewGuid(), 1, "Plan treningowy", price, 1);

        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("price");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenQuantityIsNotGreaterThanZero(int quantity)
    {
        Action act = () => new OrderItem(Guid.NewGuid(), 1, "Plan treningowy", 199m, quantity);

        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("quantity");
    }
}
