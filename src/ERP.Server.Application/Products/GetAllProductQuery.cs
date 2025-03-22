using ERP.Server.Domain.Enums;
using ERP.Server.Domain.Products;
using ERP.Server.Domain.StockMovement;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

public sealed record GetAllProductQuery() : IRequest<Result<List<GetAllProductQueryResponse>>>;

public sealed record GetAllProductQueryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ProductTypeEnum Type { get; set; } = ProductTypeEnum.Product;
    public decimal Stock { get; set; }
}

internal sealed class GetAllProductQueryHandler(
    IProductRepository productRepository,
    IStockMovementRepository stockMovementRepository) : IRequestHandler<GetAllProductQuery, Result<List<GetAllProductQueryResponse>>>
{
    public async Task<Result<List<GetAllProductQueryResponse>>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
    {
        List<Product> products = await productRepository
            .GetAll()
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        List<GetAllProductQueryResponse> response = products.Select(s => new GetAllProductQueryResponse
        {
            Id = s.Id,
            Name = s.Name,
            Type = s.Type,
            Stock = 0
        }).ToList();

        foreach (var item in response)
        {
            decimal stock = await stockMovementRepository
                .Where(p => p.ProductId == item.Id)
                .SumAsync(p => p.NumberOfEntries - p.NumberOfOutputs);

            item.Stock = stock;
        }

        return response;
    }
}
