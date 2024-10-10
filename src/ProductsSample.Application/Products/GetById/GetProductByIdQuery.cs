using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProductsSample.Abstractions.Primitives;
using ProductsSample.Abstractions.Queries;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsSample.Application.Products.GetById;

public sealed class GetProductByIdQuery : Query<Result>
{
    [BindNever]
    [NotMapped]
    public string Id { get; set; } = string.Empty;

    public override void Validate()
    {
        return;
    }
}