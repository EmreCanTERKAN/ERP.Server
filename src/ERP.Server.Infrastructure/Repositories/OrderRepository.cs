using ERP.Server.Domain.Orders;
using ERP.Server.Infrastructure.Context;
using GenericRepository;

namespace ERP.Server.Infrastructure.Repositories;

internal sealed class OrderRepository : Repository<Order, ApplicationDbContext>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
    }
}
