using ERP.Server.Domain.Orders;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Orders;

public sealed record DeleteOrderByIdCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class DeleteOrderByIdCommandHandler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteOrderByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteOrderByIdCommand request, CancellationToken cancellationToken)
    {
        Order order = await orderRepository
            .GetByExpressionAsync(o => o.Id == request.Id, cancellationToken);
        if(order is null)
        {
            return Result<string>.Failure("Bu sipariş bulunamadı");
        }
        orderRepository.Delete(order);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Sipariş başarıyla silindi";

    }
}
