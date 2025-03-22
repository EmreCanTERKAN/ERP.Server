using ERP.Server.Domain.Enums;
using ERP.Server.Domain.InvoiceDetails;
using ERP.Server.Domain.Orders;
using ERP.Server.Domain.StockMovement;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Server.Application.Invoices;
public sealed record UpdateInvoiceCommand(
    Guid Id,
    DateOnly Date,
    string InvoiceNumber,
    List<InvoiceDetailDto> Details) : IRequest<Result<string>>;

internal sealed class UpdateInvoiceCommandHandler(
    IInvoiceRepository invoiceRepository,
    IStockMovementRepository stockMovementRepository,
    IInvoiceDetailRepository invoiceDetailRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateInvoiceCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = await invoiceRepository
            .WhereWithTracking(p => p.Id == request.Id)
            .Include(p => p.Details)
            .FirstOrDefaultAsync(cancellationToken);

        if (invoice is null)
        {
            return Result<string>.Failure("Bu idye uygun kayıt bulunamadı");
        }

        List<StockMovement> movements = await stockMovementRepository
                    .Where(s => s.InvoiceId == request.Id)
                    .ToListAsync(cancellationToken);

        stockMovementRepository.DeleteRange(movements);
        invoiceDetailRepository.DeleteRange(invoice.Details);


        invoice.Date = request.Date;
        invoice.InvoiceNumber = request.InvoiceNumber;

        invoice.Details?.Clear();

        invoice.Details?.AddRange(request.Details.Select(d => new InvoiceDetail
        {
            ProductId = d.ProductId,
            Quantity = d.Quantity,
            Price = d.Price
        }));

        invoice.Details = [.. request.Details.Select(s => new InvoiceDetail
        {
            InvoiceId = invoice.Id,
            DepotId = s.DepotId,
            ProductId = s.ProductId,
            Price = s.Price,
            Quantity = s.Quantity
        })];

        await invoiceDetailRepository.AddRangeAsync(invoice.Details, cancellationToken);

        List<StockMovement> newMovements = new();

        foreach (var item in request.Details)
        {
            StockMovement movement = new()
            {
                InvoiceId = invoice.Id,
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
