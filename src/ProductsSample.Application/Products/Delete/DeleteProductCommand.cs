using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProductsSample.Abstractions.Commands;
using ProductsSample.Abstractions.Primitives;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsSample.Application.Products.Delete;

public sealed class DeleteProductCommand : Command<Result>
{
    [BindNever]
    [NotMapped]
    public string Id { get; set; } = string.Empty;

    protected override void Validate()
    {
        return;
    }
}