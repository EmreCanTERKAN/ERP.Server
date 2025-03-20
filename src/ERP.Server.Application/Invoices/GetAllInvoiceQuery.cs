using ERP.Server.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Server.Application.Invoices;
public sealed record GetAllInvoiceQuery(
    int Type) : IRequest<Result<List<Invoice>>>;

internal sealed class GetAllInvoiceQueryHandler(
    IInvoiceRepository invoiceRepository) : IRequestHandler<GetAllInvoiceQuery, Result<List<Invoice>>>
{
    public async Task<Result<List<Invoice>>> Handle(GetAllInvoiceQuery request, CancellationToken cancellationToken)
    {
        List<Invoice> invoices = await invoiceRepository
            .Where(p => p.Type == InvoiceTypeEnum.FromValue(request.Type))
            .OrderBy(p => p.Date)
            .ToListAsync(cancellationToken);

        return invoices;
    }
}
