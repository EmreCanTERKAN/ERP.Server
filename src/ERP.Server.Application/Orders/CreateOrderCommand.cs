using ERP.Server.Domain.OrderDetails;
using ERP.Server.Domain.Orders;
using GenericRepository;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Server.Application.Orders;

public sealed record CreateOrderCommand(
    Guid CustomerId,
    DateOnly Date,
    DateOnly DeliveryDate,
    List<OrderDetailDto> Details) : IRequest<Result<string>>;


public sealed record OrderDetailDto(
    Guid ProductId,
    decimal Quantity,
    decimal Price);

internal sealed class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateOrderCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // En son sipariş numarasını getir
        int lastOrderNumber = await orderRepository
            .Where(p => p.OrderNumberYear == request.Date.Year)
            .OrderByDescending(p => p.OrderNumber)
            .Select(p => p.OrderNumber)
            .FirstOrDefaultAsync(cancellationToken);

        // Mapster ile DTO -> Entity dönüşümü
        Order order = request.Adapt<Order>();
        order.OrderNumber = lastOrderNumber + 1; // Sipariş numarasını elle ekliyoruz

        // Siparişi ekleyip kaydediyoruz
        await orderRepository.AddAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Sipariş başarıyla oluşturuldu";
    }
}
