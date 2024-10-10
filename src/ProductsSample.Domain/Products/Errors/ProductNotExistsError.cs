using ProductsSample.Abstractions.Primitives;

namespace ProductsSample.Domain.Products.Errors;

public sealed class ProductNotExistsError(string message = "The product specified not exists.") 
    : BaseError(message);