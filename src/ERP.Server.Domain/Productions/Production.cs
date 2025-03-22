using ERP.Server.Domain.Depots;
using ERP.Server.Domain.Products;
using ERPServer.Domain.Abstractions;

namespace ERP.Server.Domain.Productions;
public sealed class Production : Entity
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public decimal Quantity { get; set; }
    public Guid DepotId { get; set; }
    public Depot? Depot { get; set; }
    public DateTime CreatedAt { get; set; }
}
