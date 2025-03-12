using ERP.Server.Application.Customers;
using ERP.Server.Domain.Customers;
using MediatR;
using TS.Result;

public static class CustomerModule
{
    public static void RegisterCustomerRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/customer").WithTags("Customer");

        group.MapPost("getAll",
            async (ISender sender, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new GetAllCustomerQuery(), cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<List<Customer>>>()
            .WithName("GetAllCustomers");

        group.MapPost("create",
            async (ISender sender, CreateCustomerCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>()
            .WithName("CreateCustomer");

        group.MapPost("deleteById",
            async (ISender sender, DeleteCustomerCommand request, CancellationToken     cancellationToken)  =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>()
            .WithName("DeleteCustomer");
    }
}
