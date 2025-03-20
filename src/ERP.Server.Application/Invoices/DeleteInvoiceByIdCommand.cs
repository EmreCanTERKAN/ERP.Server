using ERP.Server.Domain.StockMovement;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Server.Application.Invoices;
public sealed record DeleteInvoiceByIdCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class DeleteInvoiceByIdCommandHandler(
    IInvoiceRepository invoiceRepository,
    IStockMovementRepository stockMovementRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteInvoiceByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteInvoiceByIdCommand request, CancellationToken cancellationToken)
    {
        var invoice = await invoiceRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (invoice is null)
            return Result<string>.Failure("Bu idye uygun kayıt bulunamadı");

        List<StockMovement> movements = await stockMovementRepository
            .Where(s => s.InvoiceId == request.Id)
            .ToListAsync(cancellationToken);

        stockMovementRepository.DeleteRange(movements);

        invoiceRepository.Delete(invoice);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Fatura başarıyla silindi.";
    }
}
