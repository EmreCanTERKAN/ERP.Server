using ERP.Server.Domain.OrderDetails;
using ERP.Server.Infrastructure.Context;
using GenericRepository;

namespace ERP.Server.Infrastructure.Repositories;

internal sealed class OrderDetailRepository : Repository<OrderDetail, ApplicationDbContext>, IOrderDetailRepository
{
    public OrderDetailRepository(ApplicationDbContext context) : base(context)
    {
    }
}
