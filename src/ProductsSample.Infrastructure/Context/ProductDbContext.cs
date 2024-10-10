using Flunt.Notifications;
using Microsoft.EntityFrameworkCore;
using ProductsSample.Domain.Products;
using ProductsSample.Infrastructure.Converters;

namespace ProductsSample.Infrastructure.Context;

//public sealed class ProductDbContext(DbContextOptions options) : DbContext(options)
public class ProductDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);

        modelBuilder.Ignore(typeof(Notification));
        modelBuilder.Ignore(typeof(List<Notification>));
        modelBuilder.Ignore(typeof(IReadOnlyCollection<Notification>));

        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
           .Properties<Ulid>()
           .HaveConversion<UlidEFConverter>()
           .HaveColumnType("char(26)");

        base.ConfigureConventions(configurationBuilder);
    }
}