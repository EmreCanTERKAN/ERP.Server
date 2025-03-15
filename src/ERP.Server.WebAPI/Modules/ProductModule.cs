using ERP.Server.Application.Product;
using ERP.Server.Domain.Products;
using MediatR;
using TS.Result;

public static class ProductModule
{
    public static void RegisterProductRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/product").WithTags("Product");

        group.MapPost("getAll",
            async (ISender sender, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new GetAllProductQuery(), cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<List<Product>>>()
            .WithName("GetAllProducts");

        group.MapPost("create",
            async (ISender sender, CreateProductCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>()
            .WithName("CreateProduct");

        group.MapPost("deleteById",
            async (ISender sender, DeleteProductByIdCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>()
            .WithName("DeleteProduct");

        group.MapPost("update",
            async (ISender sender, UpdateProductCommand
            request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>()
            .WithName("UpdateProduct");
    }
}
