using ERP.Server.Domain.Enums;
using ERP.Server.Domain.InvoiceDetails;
using ERP.Server.Domain.StockMovement;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Invoices;
public sealed record CreateInvoiceCommand(
    Guid CustomerId,
    int TypeValue,
    DateOnly Date,
    string InvoiceNumber,
    List<InvoiceDetailDto> Details) : IRequest<Result<string>>;


public sealed record InvoiceDetailDto(
    Guid ProductId,
    Guid DepotId,
    decimal Quantity,
    decimal Price);

internal sealed class CreateInvoiceCommandHandler(
    IInvoiceRepository invoiceRepository,
    IStockMovementRepository stockMovementRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateInvoiceCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {

        var invoice = new Invoice
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            Date = request.Date,
            InvoiceNumber = request.InvoiceNumber,
            Type = InvoiceTypeEnum.FromValue(request.TypeValue),
            Details =
            [
                .. request.Details.Select(d => new InvoiceDetail
                    {
                        Id = Guid.NewGuid(),
                        ProductId = d.ProductId,
                        Quantity = d.Quantity,
                        Price = d.Price,
                        DepotId = d.DepotId
                    })
            ]
        };

        if (invoice.Details is not null)
        {
            List<StockMovement> movements = new();
            foreach (var item in invoice.Details)
            {
                StockMovement movement = new()
                {
                    InvoiceId = invoice.Id,
                    NumberOfEntries = request.TypeValue == 1 ? item.Quantity : 0,
                    NumberOfOutputs = request.TypeValue == 2 ? item.Quantity : 0,
                    DepotId = item.DepotId,
                    Price = item.Price,
                    ProductId = item.ProductId
                };

                movements.Add(movement);
            }
            await stockMovementRepository.AddRangeAsync(movements, cancellationToken);
        }

        await invoiceRepository.AddAsync(invoice, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Fatura başarıyla oluşturuldu";
    }
}
