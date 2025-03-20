using ERP.Server.Domain.InvoiceDetails;
using ERP.Server.Infrastructure.Context;
using GenericRepository;

namespace ERP.Server.Infrastructure.Repositories;

internal sealed class InvoiceDetailRepository : Repository<InvoiceDetail, ApplicationDbContext>, IInvoiceDetailRepository
{
    public InvoiceDetailRepository(ApplicationDbContext context) : base(context)
    {
    }
}
