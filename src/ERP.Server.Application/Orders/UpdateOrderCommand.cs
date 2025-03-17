using ERP.Server.Domain.OrderDetails;
using ERP.Server.Domain.Orders;
using GenericRepository;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Server.Application.Orders;

public sealed record UpdateOrderCommand(
    Guid Id,
    Guid CustomerId,
    DateOnly Date,
    DateOnly DeliveryDate,
    List<OrderDetailDto> Details) : IRequest<Result<string>>;

internal sealed class UpdateOrderCommandHandler(
    IOrderRepository orderRepository,
    IOrderDetailRepository orderDetailRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateOrderCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        // Güncellenecek siparişi veritabanından getir
        Order? order = await orderRepository
            .Where(o => o.Id == request.Id)
            .Include(o => o.Details)
            .FirstOrDefaultAsync(cancellationToken);

        if (order is null)
        {
            return Result<string>.Failure("Bu sipariş bulunamadı");
        }

        // Siparişin temel bilgilerini güncelle
        order.CustomerId = request.CustomerId;
        order.Date = request.Date;
        order.DeliveryDate = request.DeliveryDate;

        // Eski detayları kaldır
        orderDetailRepository.DeleteRange(order.Details);

        // Yeni detayları ekle
        order.Details = request.Details.Select(d => new OrderDetail
        {
            OrderId = order.Id, // İlişkilendirme
            ProductId = d.ProductId,
            Quantity = d.Quantity,
            Price = d.Price
            
        }).ToList();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Sipariş başarıyla güncellendi";
    }
}
