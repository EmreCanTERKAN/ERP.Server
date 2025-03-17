using ERP.Server.Domain.Depots;
using ERP.Server.Domain.Products;
using ERPServer.Domain.Abstractions;

namespace ERP.Server.Domain.StockMovement;

public sealed class StockMovement : Entity
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public Guid DepotId { get; set; }
    public decimal NumberOfEntries { get; set; }
    public decimal NumberOfOutputs { get; set; }
    public decimal Price { get; set; }
}
