using ERP.Server.Application.Depots;
using ERP.Server.Domain.Customers;
using MediatR;
using TS.Result;

public static class DepotModule
{
    public static void RegisterDepotRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/depot").WithTags("Depot");

        group.MapPost("getAll",
            async (ISender sender, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new GetAllDepotQuery(), cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<List<Customer>>>()
            .WithName("GetAllDepots");

        group.MapPost("create",
            async (ISender sender, CreateDepotCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>()
            .WithName("CreateDepot");

        group.MapPost("deleteById",
            async (ISender sender, DeleteDepotCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>()
            .WithName("DeleteDepot");

        group.MapPost("update",
            async (ISender sender, UpdateDepotCommand
            request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>()
            .WithName("UpdateDepot");
    }
}