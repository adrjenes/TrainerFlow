using FluentAssertions;
using Moq;
using TrainerFlow.Modules.Orders.Domain;
using TrainerFlow.Modules.Orders.Features.CreateOrder;
using TrainerFlow.Shared.Exceptions;

namespace TrainerFlow.Tests.Unit.Modules.Orders.CreateOrder;

public sealed class CreateOrderHandlerTests
{
    [Fact]
    public async Task HandleAsync_ShouldCreateOrderAndReturnOrderId_WhenCommandIsValid()
    {
        // Arrange
        var ordersRepositoryMock = new Mock<IOrdersRepository>();
        var orderOfferLookupMock = new Mock<IOrderOfferLookup>();

        var command = new CreateOrderCommand(
            "test@example.com",
            "Jan",
            "Kowalski",
            "123456789",
            [
                new CreateOrderItemCommand(1, 2),
                new CreateOrderItemCommand(2, 1)
            ],
            null);

        IReadOnlyList<OrderOfferDto> offers =
        [
            new(1, "Plan treningowy", 199m),
            new(2, "Plan dietetyczny", 149m)
        ];

        orderOfferLookupMock
            .Setup(x => x.GetByIdsAsync(It.IsAny<IReadOnlyCollection<int>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(offers);

        Order? savedOrder = null;

        ordersRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .Callback<Order, CancellationToken>((order, _) => savedOrder = order)
            .Returns(Task.CompletedTask);

        var handler = new CreateOrderHandler(ordersRepositoryMock.Object, orderOfferLookupMock.Object);

        // Act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();

        ordersRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()),
            Times.Once);

        savedOrder.Should().NotBeNull();
        savedOrder!.Email.Should().Be("test@example.com");
        savedOrder.FirstName.Should().Be("Jan");
        savedOrder.LastName.Should().Be("Kowalski");
        savedOrder.Phone.Should().Be("123456789");
        savedOrder.InvoiceDetail.Should().BeNull();
        savedOrder.Items.Should().HaveCount(2);

        savedOrder.Items.Should().ContainSingle(x =>
            x.OfferId == 1 &&
            x.OfferNameSnapshot == "Plan treningowy" &&
            x.Price == 199m &&
            x.Quantity == 2);

        savedOrder.Items.Should().ContainSingle(x =>
            x.OfferId == 2 &&
            x.OfferNameSnapshot == "Plan dietetyczny" &&
            x.Price == 149m &&
            x.Quantity == 1);
    }

    [Fact]
    public async Task HandleAsync_ShouldSetInvoiceDetail_WhenInvoiceDetailIsProvided()
    {
        // Arrange
        var ordersRepositoryMock = new Mock<IOrdersRepository>();
        var orderOfferLookupMock = new Mock<IOrderOfferLookup>();

        var command = new CreateOrderCommand(
            "test@example.com",
            "Jan",
            "Kowalski",
            "123456789",
            [new CreateOrderItemCommand(1, 1)],
            new CreateOrderInvoiceDetailCommand("Firma XYZ", "1234567890", "Testowa 1", "Poznan", "60-001", "Polska"));

        IReadOnlyList<OrderOfferDto> offers =
        [
            new(1, "Plan treningowy", 199m)
        ];

        orderOfferLookupMock
            .Setup(x => x.GetByIdsAsync(It.IsAny<IReadOnlyCollection<int>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(offers);

        Order? savedOrder = null;

        ordersRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .Callback<Order, CancellationToken>((order, _) => savedOrder = order)
            .Returns(Task.CompletedTask);

        var handler = new CreateOrderHandler(ordersRepositoryMock.Object, orderOfferLookupMock.Object);

        // Act
        await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        savedOrder.Should().NotBeNull();
        savedOrder!.InvoiceDetail.Should().NotBeNull();
        savedOrder.InvoiceDetail!.CompanyName.Should().Be("Firma XYZ");
        savedOrder.InvoiceDetail.TaxId.Should().Be("1234567890");
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowNotFoundException_WhenOfferDoesNotExist()
    {
        // Arrange
        var ordersRepositoryMock = new Mock<IOrdersRepository>();
        var orderOfferLookupMock = new Mock<IOrderOfferLookup>();

        var command = new CreateOrderCommand(
            "test@example.com",
            "Jan",
            "Kowalski",
            "123456789",
            [new CreateOrderItemCommand(999, 1)],
            null);

        orderOfferLookupMock
            .Setup(x => x.GetByIdsAsync(It.IsAny<IReadOnlyCollection<int>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var handler = new CreateOrderHandler(ordersRepositoryMock.Object, orderOfferLookupMock.Object);

        // Act
        Func<Task> act = () => handler.HandleAsync(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Offer with id '999' was not found.");
    }

    [Fact]
    public async Task HandleAsync_ShouldPassDistinctOfferIdsToLookup_WhenCommandContainsDuplicatedOfferIds()
    {
        // Arrange
        var ordersRepositoryMock = new Mock<IOrdersRepository>();
        var orderOfferLookupMock = new Mock<IOrderOfferLookup>();

        var command = new CreateOrderCommand(
            "test@example.com",
            "Jan",
            "Kowalski",
            "123456789",
            [
                new CreateOrderItemCommand(1, 1),
                new CreateOrderItemCommand(1, 2)
            ],
            null);

        IReadOnlyList<OrderOfferDto> offers =
        [
            new(1, "Plan treningowy", 199m)
        ];

        orderOfferLookupMock
            .Setup(x => x.GetByIdsAsync(It.IsAny<IReadOnlyCollection<int>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(offers);

        var handler = new CreateOrderHandler(ordersRepositoryMock.Object, orderOfferLookupMock.Object);

        // Act
        await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        orderOfferLookupMock.Verify(
            x => x.GetByIdsAsync(
                It.Is<IReadOnlyCollection<int>>(ids => ids.Count == 1 && ids.Contains(1)),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
