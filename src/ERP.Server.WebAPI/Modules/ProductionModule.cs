using ERP.Server.Application.Productions;
using ERP.Server.Domain.Productions;
using MediatR;
using TS.Result;

public static class ProductionModule
{
    public static void RegisterProductionRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/production").WithTags("Production");

        group.MapPost("getAll",
            async (ISender sender, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new GetAllProductionQuery(), cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<List<Production>>>()
            .WithName("GetAllProductions");

        group.MapPost("create",
            async (ISender sender, CreateProductionCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>()
            .WithName("CreateProduction");

        group.MapPost("deleteById",
            async (ISender sender, DeleteProductionByIdCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>()
            .WithName("DeleteProduction");


    }
}
