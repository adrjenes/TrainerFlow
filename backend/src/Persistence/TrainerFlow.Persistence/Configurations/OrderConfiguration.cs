using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainerFlow.Modules.Orders.Domain;

namespace TrainerFlow.Persistence.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.UserId);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Phone)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(x => x.CreatedUtc)
            .IsRequired();

        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.Email);

        builder.OwnsOne(x => x.InvoiceDetail, invoice =>
        {
            invoice.Property(x => x.CompanyName)
                .HasColumnName("invoice_company_name")
                .HasMaxLength(100);

            invoice.Property(x => x.TaxId)
                .HasColumnName("invoice_tax_id")
                .HasMaxLength(12);

            invoice.Property(x => x.Address)
                .HasColumnName("invoice_address")
                .HasMaxLength(70);

            invoice.Property(x => x.City)
                .HasColumnName("invoice_city")
                .HasMaxLength(50);

            invoice.Property(x => x.PostalCode)
                .HasColumnName("invoice_postal_code")
                .HasMaxLength(6);

            invoice.Property(x => x.Country)
                .HasColumnName("invoice_country")
                .HasMaxLength(100);
        });

        builder.Navigation(x => x.InvoiceDetail).IsRequired(false);

        builder.Navigation(x => x.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}