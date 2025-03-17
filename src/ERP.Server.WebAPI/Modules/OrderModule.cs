using ERP.Server.Application.Orders;
using ERP.Server.Domain.Orders;
using MediatR;
using TS.Result;

public static class OrderModule
{
    public static void RegisterOrderRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/order").WithTags("Order");

        group.MapPost("getAll",
            async (ISender sender, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new GetAllOrderQuery(), cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<List<Order>>>()
            .WithName("GetAllOrders");

        group.MapPost("create",
            async (ISender sender, CreateOrderCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>()
            .WithName("CreateOrder");

        group.MapPost("deleteById",
            async (ISender sender, DeleteOrderByIdCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>()
            .WithName("DeleteOrder");

        group.MapPost("update",
            async (ISender sender, UpdateOrderCommand
            request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>()
            .WithName("UpdateOrder");
    }
}
