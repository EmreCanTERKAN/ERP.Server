using ERP.Server.Domain.Orders;
using ERP.Server.Domain.Products;
using ERP.Server.Domain.RecipeDetails;
using ERP.Server.Domain.Recipes;
using ERP.Server.Domain.StockMovement;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Server.Application.Orders;

public sealed record RequirementsPlanningByOrderIdCommand(
    Guid OrderId) : IRequest<Result<RequirementsPlanningByOrderIdCommandResponse>>;

public sealed record RequirementsPlanningByOrderIdCommandResponse(
    DateOnly Date,
    string Title,
    List<ProductDto> Products);

public sealed record ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
}


internal sealed class RequirementsPlanningByOrderIdCommandHandler(
    IOrderRepository orderRepository,
    IStockMovementRepository stockMovementRepository,
    IRecipeRepository recipeRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RequirementsPlanningByOrderIdCommand, Result<RequirementsPlanningByOrderIdCommandResponse>>
{
    public async Task<Result<RequirementsPlanningByOrderIdCommandResponse>> Handle(RequirementsPlanningByOrderIdCommand request, CancellationToken cancellationToken)
    {
        Order? order = await orderRepository
            .Where(o => o.Id == request.OrderId)
            .Include(p => p.Details!)
            .ThenInclude(p => p.Product)
            .FirstOrDefaultAsync(cancellationToken);

        if (order is null)
        {
            return Result<RequirementsPlanningByOrderIdCommandResponse>.Failure("Sipariş Bulunamadı");
        }

        List<ProductDto> uretilmesiGerekenUrunListesi = new();
        List<ProductDto> ihtiyacOlanUrunListesi = new();

        if (order.Details is not null)
        {
            foreach (var item in order.Details)
            {
                var product = item.Product;

                List<StockMovement> movements = await stockMovementRepository
                    .Where(p => p.ProductId == product!.Id)
                    .ToListAsync(cancellationToken);

                decimal stock = movements
                    .Sum(p => p.NumberOfEntries) - movements.Sum(p => p.NumberOfOutputs);

                if (stock < item.Quantity)
                {
                    ProductDto uretilmesiGerekenUrun = new()
                    {
                        Id = item.ProductId,
                        Name = product!.Name,
                        Quantity = item.Quantity - stock
                    };

                    uretilmesiGerekenUrunListesi.Add(uretilmesiGerekenUrun);
                }
            }

            foreach (var item in uretilmesiGerekenUrunListesi)
            {
                Recipe? recipe = await recipeRepository
                    .Where(p => p.ProductId == item.Id)
                    .Include(p => p.Details!)
                    .ThenInclude(p => p.Product)
                    .FirstOrDefaultAsync(cancellationToken);

                if(recipe is not null && recipe.Details is not null)
                {
                    foreach(var detail in recipe.Details)
                    {
                        List<StockMovement> urunHareketleri = await stockMovementRepository
                               .Where(p => p.ProductId == detail.ProductId)
                               .ToListAsync(cancellationToken);

                        decimal stock = urunHareketleri
                            .Sum(p => p.NumberOfEntries) - urunHareketleri.Sum(p => p.NumberOfOutputs);
                        if(stock < detail.Quantity)
                        {
                            ProductDto ihtiyacOlanUrun = new()
                            {
                                Id = detail.Id,
                                Name = detail.Product!.Name,
                                Quantity = detail.Quantity - stock
                            };
                            ihtiyacOlanUrunListesi.Add(ihtiyacOlanUrun);
                        }
                    }
                }
            }
        }

        ihtiyacOlanUrunListesi = ihtiyacOlanUrunListesi
            .GroupBy(p => p.Id)
            .Select(p => new ProductDto
            {
                Id = p.Key,
                Name = p.First().Name,
                Quantity = p.Sum(item => item.Quantity)
            }).ToList();

        order.Status = Domain.Enums.OrderStatusEnum.RequirementsPlanWorked;
        orderRepository.Update(order);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new RequirementsPlanningByOrderIdCommandResponse(DateOnly.FromDateTime(DateTime.Now), order.Number + "Nolu Siparişin ihtiyaç planlaması", ihtiyacOlanUrunListesi);
    }
}
