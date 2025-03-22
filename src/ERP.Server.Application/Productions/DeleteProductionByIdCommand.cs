using ERP.Server.Domain.Productions;
using ERP.Server.Domain.StockMovement;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Server.Application.Productions;
public sealed record DeleteProductionByIdCommand(
    Guid Id) :  IRequest<Result<string>>;

internal sealed class DeleteProductionByIdCommandHandler(
    IProductionRepository productionRepository,
    IStockMovementRepository stockMovementRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteProductionByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteProductionByIdCommand request, CancellationToken cancellationToken)
    {
        Production? production = await productionRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.Id);
        if (production is null)
        {
            return Result<string>.Failure("Üretim Bulunamadı");
        }

        List<StockMovement> movements = await stockMovementRepository.Where(p => p.ProductId == production.Id).ToListAsync(cancellationToken);

        stockMovementRepository.DeleteRange(movements);

        productionRepository.Delete(production);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Üretim başarıyla silindi";

    }
}


