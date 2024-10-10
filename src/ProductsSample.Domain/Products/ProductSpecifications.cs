using Microsoft.EntityFrameworkCore;
using ProductsSample.Abstractions.Enums;
using System.Linq.Expressions;

namespace ProductsSample.Domain.Products;

public static class ProductSpecifications
{
    public static Expression<Func<Product, bool>> ByActive()
    {
        return product => product.Status == EntityStatus.Active;
    }

    public static Expression<Func<Product, bool>> ById(Ulid id)
    {
        return product => product.Id == id && product.Status == EntityStatus.Active;
    }

    public static Expression<Func<Product, bool>> ByExactName(string name)
    {
        return product => product.Name.ToLower() == name.ToLower() && product.Status == EntityStatus.Active;
    }

    public static Expression<Func<Product, bool>> ByName(string name)
    {
        return product => product.Name.Contains(name) && product.Status == EntityStatus.Active;
    }

    public static Expression<Func<Product, bool>> ByMinimumPrice(decimal minimumPrice)
    {
        return product => product.Price.Value >= minimumPrice && product.Status == EntityStatus.Active;
    }

    public static Expression<Func<Product, bool>> ByMaximumPrice(decimal maximumPrice)
    {
        return product => product.Price.Value <= maximumPrice && product.Status == EntityStatus.Active;
    }

    public static Expression<Func<Product, bool>> ByMinimumStock(int minimumStock)
    {
        return product => product.Stock.Value >= minimumStock && product.Status == EntityStatus.Active;
    }

    public static Expression<Func<Product, bool>> ByMaximumStock(int maximumStock)
    {
        return product => product.Stock.Value <= maximumStock && product.Status == EntityStatus.Active;
    }
}