using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsSample.Domain.Products;

namespace ProductsSample.Infrastructure.Configurations;

public sealed class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.OwnsOne(x => x.Price, price =>
        {
            price.Property(x => x.Value)
                .HasPrecision(10, 2)
                .IsRequired();
        });

        builder.OwnsOne(x => x.Stock, stock =>
        {
            stock.Property(x => x.Value)
                .IsRequired();
        });
    }
}