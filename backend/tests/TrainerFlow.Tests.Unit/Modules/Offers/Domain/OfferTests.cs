using FluentAssertions;
using TrainerFlow.Modules.Offers.Domain;

namespace TrainerFlow.Tests.Unit.Modules.Offers.Domain;

public sealed class OfferTests
{
    [Fact]
    public void Constructor_ShouldCreateOffer_WhenDataIsValid()
    {
        var offer = new Offer("plan-treningowy", "Plan treningowy", 199m);

        offer.Slug.Should().Be("plan-treningowy");
        offer.Name.Should().Be("Plan treningowy");
        offer.Price.Should().Be(199m);
    }

    [Fact]
    public void Constructor_ShouldNormalizeSlug_WhenSlugContainsSpacesAndUppercase()
    {
        var offer = new Offer("  PLAN-TRENINGOWY  ", "Plan treningowy", 199m);

        offer.Slug.Should().Be("plan-treningowy");
    }

    [Fact]
    public void Constructor_ShouldTrimName_WhenNameContainsSpaces()
    {
        var offer = new Offer("plan", "  Plan treningowy  ", 199m);

        offer.Name.Should().Be("Plan treningowy");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenSlugIsEmpty()
    {
        Action act = () => new Offer(" ", "Plan treningowy", 199m);

        act.Should().Throw<ArgumentException>()
            .WithParameterName("slug");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        Action act = () => new Offer("plan-treningowy", " ", 199m);

        act.Should().Throw<ArgumentException>()
            .WithParameterName("name");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenPriceIsNotGreaterThanZero(decimal price)
    {
        Action act = () => new Offer("plan-treningowy", "Plan treningowy", price);

        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("price");
    }

    [Fact]
    public void ChangePrice_ShouldUpdatePrice_WhenPriceIsValid()
    {
        var offer = new Offer("plan-treningowy", "Plan treningowy", 199m);

        offer.ChangePrice(299m);

        offer.Price.Should().Be(299m);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void ChangePrice_ShouldThrowArgumentOutOfRangeException_WhenPriceIsNotGreaterThanZero(decimal price)
    {
        var offer = new Offer("plan-treningowy", "Plan treningowy", 199m);
        Action act = () => offer.ChangePrice(price);

        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("price");
    }
}