namespace TrainerFlow.Modules.Offers.Domain;

public sealed class Offer
{
    public int Id { get; private set; }
    public string Slug { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public decimal Price { get; private set; }

    private Offer() { }

    public Offer(string slug, string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(slug)) throw new ArgumentException("Slug is required.", nameof(slug));

        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));

        if (price < 0) throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");

        Slug = slug.Trim().ToLowerInvariant();
        Name = name.Trim();
        Price = price;
    }

    public void ChangePrice(decimal price)
    {
        if (price < 0) throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");

        Price = price;
    }
}