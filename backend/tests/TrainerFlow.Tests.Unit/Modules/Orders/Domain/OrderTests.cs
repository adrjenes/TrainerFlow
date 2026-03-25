using FluentAssertions;
using TrainerFlow.Modules.Orders.Domain;

namespace TrainerFlow.Tests.Unit.Modules.Orders.Domain;

public sealed class OrderTests
{
    [Fact]
    public void Constructor_ShouldCreateOrder_WhenDataIsValid()
    {
        var order = new Order("test@example.com", "Jan", "Kowalski", "123456789");

        order.Email.Should().Be("test@example.com");
        order.FirstName.Should().Be("Jan");
        order.LastName.Should().Be("Kowalski");
        order.Phone.Should().Be("123456789");
        order.Items.Should().BeEmpty();
        order.InvoiceDetail.Should().BeNull();
    }

    [Fact]
    public void Constructor_ShouldNormalizeEmail_WhenEmailContainsSpacesAndUppercase()
    {
        var order = new Order("  TEST@EXAMPLE.COM  ", "Jan", "Kowalski", "123456789");

        order.Email.Should().Be("test@example.com");
    }

    [Fact]
    public void Constructor_ShouldTrimFirstNameLastNameAndPhone_WhenValuesContainSpaces()
    {
        var order = new Order("test@example.com", "  Jan  ", "  Kowalski  ", "  123456789  ");

        order.FirstName.Should().Be("Jan");
        order.LastName.Should().Be("Kowalski");
        order.Phone.Should().Be("123456789");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenEmailIsEmpty()
    {
        Action act = () => new Order(" ", "Jan", "Kowalski", "123456789");

        act.Should().Throw<ArgumentException>()
            .WithParameterName("email");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenFirstNameIsEmpty()
    {
        Action act = () => new Order("test@example.com", " ", "Kowalski", "123456789");

        act.Should().Throw<ArgumentException>()
            .WithParameterName("firstName");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenLastNameIsEmpty()
    {
        Action act = () => new Order("test@example.com", "Jan", " ", "123456789");

        act.Should().Throw<ArgumentException>()
            .WithParameterName("lastName");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenPhoneIsEmpty()
    {
        Action act = () => new Order("test@example.com", "Jan", "Kowalski", " ");

        act.Should().Throw<ArgumentException>()
            .WithParameterName("phone");
    }

    [Fact]
    public void AssignUser_ShouldAssignUser_WhenUserIdIsValid()
    {
        var order = new Order("test@example.com", "Jan", "Kowalski", "123456789");
        var userId = Guid.NewGuid();

        order.AssignUser(userId);

        order.UserId.Should().Be(userId);
    }

    [Fact]
    public void AssignUser_ShouldThrowArgumentException_WhenUserIdIsEmpty()
    {
        var order = new Order("test@example.com", "Jan", "Kowalski", "123456789");

        Action act = () => order.AssignUser(Guid.Empty);

        act.Should().Throw<ArgumentException>()
            .WithParameterName("userId");
    }

    [Fact]
    public void AssignUser_ShouldThrowInvalidOperationException_WhenUserAlreadyAssigned()
    {
        var order = new Order("test@example.com", "Jan", "Kowalski", "123456789");
        order.AssignUser(Guid.NewGuid());

        Action act = () => order.AssignUser(Guid.NewGuid());

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("User already assigned to this order.");
    }

    [Fact]
    public void AddItem_ShouldAddItemToCollection_WhenDataIsValid()
    {
        var order = new Order("test@example.com", "Jan", "Kowalski", "123456789");

        order.AddItem(1, "Plan treningowy", 199m, 2);

        order.Items.Should().HaveCount(1);

        var item = order.Items.Single();
        item.OfferId.Should().Be(1);
        item.OfferNameSnapshot.Should().Be("Plan treningowy");
        item.Price.Should().Be(199m);
        item.Quantity.Should().Be(2);
        item.OrderId.Should().Be(order.Id);
    }

    [Fact]
    public void SetInvoiceDetail_ShouldSetInvoiceDetail_WhenDataIsValid()
    {
        var order = new Order("test@example.com", "Jan", "Kowalski", "123456789");

        order.SetInvoiceDetail("Firma XYZ", "1234567890", "Testowa 1", "Poznan", "60-001", "Polska");

        order.InvoiceDetail.Should().NotBeNull();
        order.InvoiceDetail!.CompanyName.Should().Be("Firma XYZ");
        order.InvoiceDetail.TaxId.Should().Be("1234567890");
        order.InvoiceDetail.Address.Should().Be("Testowa 1");
        order.InvoiceDetail.City.Should().Be("Poznan");
        order.InvoiceDetail.PostalCode.Should().Be("60-001");
        order.InvoiceDetail.Country.Should().Be("Polska");
    }
}