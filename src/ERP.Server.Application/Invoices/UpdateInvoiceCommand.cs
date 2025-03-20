using ERP.Server.Domain.Enums;
using ERP.Server.Domain.InvoiceDetails;
using ERP.Server.Domain.StockMovement;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Server.Application.Invoices;
public sealed record UpdateInvoiceCommand(
    Guid Id,
    Guid CustomerId,
    int TypeValue,
    DateOnly Date,
    string InvoiceNumber,
    List<InvoiceDetailDto> Details) : IRequest<Result<string>>;

internal sealed class UpdateInvoiceCommandHandler(
    IInvoiceRepository invoiceRepository,
    IStockMovementRepository stockMovementRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateInvoiceCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = await invoiceRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

        if (invoice is null)
        {
            return Result<string>.Failure("Bu idye uygun kayıt bulunamadı");
        }

        List<StockMovement> movements = await stockMovementRepository
                    .Where(s => s.InvoiceId == request.Id)
                    .ToListAsync(cancellationToken);

        stockMovementRepository.DeleteRange(movements);

        invoice.CustomerId = request.CustomerId;
        invoice.Type = InvoiceTypeEnum.FromValue(request.TypeValue);
        invoice.Date = request.Date;
        invoice.InvoiceNumber = request.InvoiceNumber;

        invoice.Details?.Clear();

        invoice.Details?.AddRange(request.Details.Select(d => new InvoiceDetail
        {
            ProductId = d.ProductId,
            Quantity = d.Quantity,
            Price = d.Price
        }));

        List<StockMovement> newMovements = new();

        foreach (var item in request.Details)
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

            newMovements.Add(movement);
        }
        await stockMovementRepository.AddRangeAsync(newMovements, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Fatura başarıyla güncellendi";

    }
}
