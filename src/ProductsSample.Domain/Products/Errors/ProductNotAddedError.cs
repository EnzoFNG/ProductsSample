using ProductsSample.Abstractions.Primitives;

namespace ProductsSample.Domain.Products.Errors;

public sealed class ProductNotAddedError(string message = "An error has ocurred while added the Product.") 
    : BaseError(message);