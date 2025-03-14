using ERP.Server.Domain.Products;
using ERPServer.Domain.Abstractions;

namespace ERP.Server.Domain.RecipeDetails;

public sealed class RecipeDetail : Entity
{
    public Guid RecipeId { get; set; }
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public decimal Quantity { get; set; }
}
