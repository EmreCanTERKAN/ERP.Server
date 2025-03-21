using ERP.Server.Domain.Productions;
using ERP.Server.Infrastructure.Context;
using GenericRepository;

namespace ERP.Server.Infrastructure.Repositories;

internal sealed class ProductionRepository : Repository<Production, ApplicationDbContext>, IProductionRepository
{
    public ProductionRepository(ApplicationDbContext context) : base(context)
    {
    }
}
