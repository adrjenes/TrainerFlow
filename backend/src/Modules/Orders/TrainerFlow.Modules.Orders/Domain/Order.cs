namespace TrainerFlow.Modules.Orders.Domain;

public sealed class Order
{
    public Guid Id { get; private set; }
    public Guid? UserId { get; private set; }
    public string Email { get; private set; } = null!;
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string Phone { get; private set; } = null!;
    public DateTime CreatedUtc { get; private set; }

    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items;
    public OrderInvoiceDetail? InvoiceDetail { get; private set; }

    private Order() { }

    public Order(string email, string firstName, string lastName, string phone)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required.", nameof(email));

        if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name is required.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name is required.", nameof(lastName));

        if (string.IsNullOrWhiteSpace(phone)) throw new ArgumentException("Phone is required.", nameof(phone));

        Id = Guid.CreateVersion7();
        Email = email.Trim().ToLowerInvariant();
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Phone = phone.Trim();
        CreatedUtc = DateTime.UtcNow;
    }

    public void AssignUser(Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentException("UserId is required.", nameof(userId));

        if (UserId.HasValue) throw new InvalidOperationException("User already assigned to this order.");

        UserId = userId;
    }

    public void AddItem(int offerId, string offerNameSnapshot, decimal price, int quantity)
    {
        var item = new OrderItem(Id, offerId, offerNameSnapshot, price, quantity);
        _items.Add(item);
    }

    public void SetInvoiceDetail(string companyName, string taxId, string address, string city, string postalCode, string country)
    {
        InvoiceDetail = new OrderInvoiceDetail(companyName, taxId, address, city, postalCode, country);
    }
}