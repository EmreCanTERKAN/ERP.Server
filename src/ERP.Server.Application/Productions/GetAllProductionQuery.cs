using ERP.Server.Domain.Productions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Server.Application.Productions;
public sealed record class GetAllProductionQuery() : IRequest<Result<List<Production>>>;

internal sealed class GetAllProductionQueryHandler(
    IProductionRepository productionRepository) : IRequestHandler<GetAllProductionQuery, Result<List<Production>>>
{
    public async Task<Result<List<Production>>> Handle(GetAllProductionQuery request, CancellationToken cancellationToken)
    {
        var productions = await productionRepository
            .GetAll()
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
        return productions;

    }
}
