using Bogus;
using ProductsSample.Domain.Products;
using ProductsSample.Infrastructure.Context;

namespace ProductsSample.Infrastructure.Seeding;

public static class ProductSeed
{
    public static void SeedIfNotEmpty(ProductDbContext dbContext)
    {
        var dataExists = dbContext.Products.Any();

        if (dataExists)
            return;

        var products = GenerateData();  

        dbContext.Products.AddRangeAsync(products);
        dbContext.SaveChangesAsync();
    }

    private static List<Product> GenerateData()
    {
        var productFaker = new Faker<Product>()
            .CustomInstantiator(faker =>
            {
                var productPrice = decimal.Parse(faker.Commerce.Price(min: 10, max: 10000, decimals: 2));

                var result = Product.Create(
                    faker.Commerce.ProductName(),
                    productPrice,
                    faker.Random.UInt(0, 1000));

                return (Product)result.Response!;
            });

        var products = productFaker.Generate(50);

        return products;
    }
}