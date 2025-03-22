using ERP.Server.Domain.Productions;
using ERP.Server.Domain.Recipes;
using ERP.Server.Domain.StockMovement;
using GenericRepository;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Server.Application.Productions;
public sealed record CreateProductionCommand(
    Guid ProductId,
    Guid DepotId,
    decimal Quantity) : IRequest<Result<string>>;

internal sealed class CreateProductionCommandHandler(
    IProductionRepository productionRepository,
    IRecipeRepository recipeRepository,
    IStockMovementRepository stockMovementRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateProductionCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateProductionCommand request, CancellationToken cancellationToken)
    {
        var production = new Production
        {
            ProductId = request.ProductId,
            DepotId = request.DepotId,
            Quantity = request.Quantity,
            CreatedAt = DateTime.UtcNow
        };

        List<StockMovement> newMovements = new();

        Recipe? recipe =
            await recipeRepository
            .Where(p => p.ProductId == request.ProductId)
            .Include(p => p.Details!)
            .ThenInclude(p => p.Product)
            .FirstOrDefaultAsync(cancellationToken);

        if (recipe is not null && recipe.Details is not null)
        {
            var details = recipe.Details;

            foreach (var item in details)
            {
                List<StockMovement> movements = await stockMovementRepository.Where(p => p.ProductId == item.ProductId).ToListAsync(cancellationToken);

                List<Guid> depotIds = movements.GroupBy(p => p.DepotId)
                    .Select(g => g.Key)
                    .ToList();

                decimal stock = movements.Sum(p => p.NumberOfEntries) - movements.Sum(p => p.NumberOfOutputs);

                if (item.Quantity > stock)
                {
                    return Result<string>.Failure(item.Product!.Name + " ürününden üretim için yeterli miktarda yok. Eksik miktar: " + (item.Quantity - stock));
                }

                foreach (var depotId in depotIds)
                {
                    if (item.Quantity <= 0) break;

                    decimal quantity = movements.Where(p => p.DepotId == depotId).Sum(s => s.NumberOfEntries - s.NumberOfOutputs);

                    decimal totalAmount =
                        movements
                        .Where(p => p.DepotId == depotId && p.NumberOfEntries > 0)
                        .Sum(s => s.Price * s.NumberOfEntries);

                    decimal totalEntiresQuantity =
                        movements
                        .Where(p => p.DepotId == depotId && p.NumberOfEntries > 0)
                        .Sum(s => s.NumberOfEntries);

                    decimal price = totalAmount / totalEntiresQuantity;


                    StockMovement stockMovement = new()
                    {
                        ProductionId = production.Id,
                        ProductId = item.ProductId,
                        DepotId = depotId,
                        Price = price
                    };

                    if (item.Quantity <= quantity)
                    {
                        stockMovement.NumberOfOutputs = item.Quantity;
                    }
                    else
                    {
                        stockMovement.NumberOfOutputs = quantity;
                    }

                    item.Quantity -= quantity;

                    newMovements.Add(stockMovement);
                }
            }
        }

        await stockMovementRepository.AddRangeAsync(newMovements, cancellationToken);
        await productionRepository.AddAsync(production, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Ürün başarıyla üretildi";
    }
}



