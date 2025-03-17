using ERP.Server.Domain.Orders;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Server.Application.Orders;

public sealed record GetAllOrderQuery() : IRequest<Result<List<Order>>>;


internal sealed class GetAllOrderQueryHandler(
    IOrderRepository orderRepository) : IRequestHandler<GetAllOrderQuery, Result<List<Order>>>
{
    public async Task<Result<List<Order>>> Handle(GetAllOrderQuery request, CancellationToken cancellationToken)
    {
        List<Order> orders =
            await orderRepository
            .GetAll()
            .Include(p => p.Customer)
            .Include(p => p.Details!)
            .ThenInclude(p => p.Product)
            .AsSplitQuery()
            .OrderByDescending(p => p.Date)
            .ToListAsync(cancellationToken);
        return orders; 
    }
}
