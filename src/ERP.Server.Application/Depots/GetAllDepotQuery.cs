using ERP.Server.Domain.Depots;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Server.Application.Depots;

public sealed record GetAllDepotQuery() : IRequest<Result<List<Depot>>>;

internal sealed class GetAllDepotQueryHandler(
    IDepotRepository depotRepository) : IRequestHandler<GetAllDepotQuery, Result<List<Depot>>>
{
    public async Task<Result<List<Depot>>> Handle(GetAllDepotQuery request, CancellationToken cancellationToken)
    {
        List<Depot> depots = await depotRepository.GetAll().OrderBy(p => p.Name).ToListAsync(cancellationToken);
        return depots;
    }
}