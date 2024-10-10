using ProductsSample.Abstractions.Primitives;

namespace ProductsSample.Domain.Products.Errors;

public sealed class ProductNotUpdatedError(string message = "An error has ocurred while updating the Product.")
    : BaseError(message);