using ERP.Server.Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

public sealed record GetAllProductQuery() : IRequest<Result<List<Product>>>;


internal sealed class GetAllProductQueryHandler(
    IProductRepository productRepository) : IRequestHandler<GetAllProductQuery, Result<List<Product>>>
{
    public async Task<Result<List<Product>>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
    {
        List<Product> products = await productRepository
            .GetAll()
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
        return products;
    }
}
