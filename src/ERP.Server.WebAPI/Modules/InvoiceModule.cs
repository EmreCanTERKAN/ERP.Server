using ERP.Server.Application.Invoices;
using MediatR;
using TS.Result;

public static class InvoiceModule
{
    public static void RegisterDepotRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/invoice").WithTags("Invoice");

        group.MapPost("getAll",
            async (GetAllInvoiceQuery request,ISender sender, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<List<Invoice>>>()
            .WithName("GetAllInvoice");

        group.MapPost("create",
            async (ISender sender, CreateInvoiceCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>()
            .WithName("CreateInvoice");

        group.MapPost("deleteById",
            async (ISender sender, DeleteInvoiceByIdCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>()
            .WithName("DeleteInvoiceById");

        group.MapPost("update",
            async (ISender sender, UpdateInvoiceCommand
            request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>()
            .WithName("UpdateInvoice");
    }
}