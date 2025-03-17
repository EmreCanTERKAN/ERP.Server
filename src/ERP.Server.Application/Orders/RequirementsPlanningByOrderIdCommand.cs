using ERP.Server.Domain.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Server.Application.Orders;

public sealed record RequirementsPlanningByOrderIdCommand(
    Guid Id) : IRequest<Result<RequirementsPlanningByOrderIdCommandResponse>>;

public sealed record RequirementsPlanningByOrderIdCommandResponse(
    DateOnly Date,
    string Title,
    List<ProductDto> Products);

public sealed record ProductDto(
    string Name,
    decimal Quantity);


internal sealed class RequirementsPlanningByOrderIdCommandHandler(
    IOrderRepository orderRepository) : IRequestHandler<RequirementsPlanningByOrderIdCommand, Result<RequirementsPlanningByOrderIdCommandResponse>>
{
    public async Task<Result<RequirementsPlanningByOrderIdCommandResponse>> Handle(RequirementsPlanningByOrderIdCommand request, CancellationToken cancellationToken)
    {
        Order? order = await orderRepository
            .Where(o => o.Id == request.Id)
            .Include(p => p.Details)
            .FirstOrDefaultAsync(cancellationToken);

        if (order is null)
        {
            return Result<RequirementsPlanningByOrderIdCommandResponse>.Failure("Ürün Bulunamadı");
        }

        //todo
        return new RequirementsPlanningByOrderIdCommandResponse(DateOnly.FromDateTime(DateTime.Now), order.Number + "Nolu Siparişin ihtiyaç planlaması", new());
    }
}
