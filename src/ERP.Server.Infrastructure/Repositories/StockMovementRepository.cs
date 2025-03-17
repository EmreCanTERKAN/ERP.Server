using ERP.Server.Domain.StockMovement;
using ERP.Server.Infrastructure.Context;
using GenericRepository;

namespace ERP.Server.Infrastructure.Repositories;

internal sealed class StockMovementRepository : Repository<StockMovement, ApplicationDbContext>, IStockMovementRepository
{
    public StockMovementRepository(ApplicationDbContext context) : base(context)
    {
    }
}
