using ProductsSample.Abstractions.Primitives;

namespace ProductsSample.Domain.Products.Errors;

public sealed class ProductAlreadyExistsError(string message = "The product with this name already exists.")
    : BaseError(message);