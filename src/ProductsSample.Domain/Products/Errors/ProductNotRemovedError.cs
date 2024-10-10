using ProductsSample.Abstractions.Primitives;

namespace ProductsSample.Domain.Products.Errors;

public sealed class ProductNotRemovedError(string message = "An error has ocurred while removing the Product.") 
    : BaseError(message);