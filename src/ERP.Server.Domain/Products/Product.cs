using ERP.Server.Domain.Enums;
using ERPServer.Domain.Abstractions;

namespace ERP.Server.Domain.Products;

public sealed class Product : Entity
{
    public string Name { get; set; } = string.Empty;
    public ProductTypeEnum Type { get; set; } = ProductTypeEnum.Product;
}
