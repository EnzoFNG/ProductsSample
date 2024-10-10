using Microsoft.EntityFrameworkCore;
using ProductsSample.Infrastructure.Context;
using ProductsSample.Infrastructure.Seeding;

namespace ProductsSample.Api.Configurations;

public static class DatabaseConfiguration
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddDbContext<ProductDbContext>(options => options.UseInMemoryDatabase("ProductsDB"));

        services.SeedData();

        return services;
    }

    private static void SeedData(this IServiceCollection services)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        if (environment is "Production")
            return;

        var serviceProvider = services.BuildServiceProvider();

        var dbContext = serviceProvider.GetRequiredService<ProductDbContext>();

        ProductSeed.SeedIfNotEmpty(dbContext);
    }
}