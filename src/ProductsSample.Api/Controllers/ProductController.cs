using Microsoft.AspNetCore.Mvc;
using ProductsSample.Abstractions.Controllers;
using ProductsSample.Abstractions.Mediator;
using ProductsSample.Application.Products.AddNew;
using ProductsSample.Application.Products.Delete;
using ProductsSample.Application.Products.EditDetails;
using ProductsSample.Application.Products.GetById;
using ProductsSample.Application.Products.ListAll;
using Swashbuckle.AspNetCore.Annotations;

namespace ProductsSample.Api.Controllers;

[Route("api/v1/products")]
public sealed class ProductController(IMediatorHandler mediator) : CustomController
{
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Displays a product by its code.",
        Description = "Endpoint used to retrieve a product by its code.")]
    public async Task<IActionResult> GetProductByIdAsync(string id)
    {
        var response = await mediator.SendQueryAsync(new GetProductByIdQuery
        {
            Id = id
        });

        return CustomResponse(response);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Lists all products.",
        Description = "Endpoint used to list all products.")]
    public async Task<IActionResult> ListAllProductsAsync([FromQuery] ListAllProductsQuery query)
    {
        var response = await mediator.SendQueryAsync(query);

        return CustomResponse(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Registers a new product.",
        Description = "Endpoint used to add a new product.")]
    public async Task<IActionResult> AddNewProductAsync([FromBody] AddNewProductCommand command)
    {
        var response = await mediator.SendCommandAsync(command);

        return CustomResponse(response);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Updates a product's details.",
        Description = "Endpoint used to update an existing product's details by its ID.")]
    public async Task<IActionResult> EditProductDetailsAsync(string id, [FromBody] EditProductDetailsCommand command)
    {
        command.Id = id;
        var response = await mediator.SendCommandAsync(command);

        return CustomResponse(response);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Deletes a product.",
        Description = "Endpoint used to delete a product by its ID.")]
    public async Task<IActionResult> DeleteProductAsync(string id)
    {
        var response = await mediator.SendCommandAsync(
            new DeleteProductCommand
            {
                Id = id
            });

        return CustomResponse(response);
    }
}