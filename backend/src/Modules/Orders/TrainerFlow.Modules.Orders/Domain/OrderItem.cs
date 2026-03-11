namespace TrainerFlow.Modules.Orders.Domain;

public sealed class OrderItem
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public int OfferId { get; private set; }
    public string OfferNameSnapshot { get; private set; } = null!;
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }

    private OrderItem() { }

    public OrderItem(Guid orderId, int offerId, string offerNameSnapshot, decimal price, int quantity)
    {
        if (orderId == Guid.Empty) throw new ArgumentException("OrderId is required.", nameof(orderId));

        if (offerId <= 0) throw new ArgumentOutOfRangeException(nameof(offerId), "OfferId must be greater than zero.");

        if (string.IsNullOrWhiteSpace(offerNameSnapshot)) throw new ArgumentException("Offer name snapshot is required.", nameof(offerNameSnapshot));

        if (price < 0) throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");

        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");

        Id = Guid.CreateVersion7();
        OrderId = orderId;
        OfferId = offerId;
        OfferNameSnapshot = offerNameSnapshot.Trim();
        Price = price;
        Quantity = quantity;
    }
}