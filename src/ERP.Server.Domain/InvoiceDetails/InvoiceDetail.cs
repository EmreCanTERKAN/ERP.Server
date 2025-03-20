﻿using ERP.Server.Domain.Depots;
using ERP.Server.Domain.Products;
using ERPServer.Domain.Abstractions;

namespace ERP.Server.Domain.InvoiceDetails;
public sealed class InvoiceDetail : Entity
{
    public Guid InvoiceId { get; set; }
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public Guid DepotId { get; set; }
    public Depot? Depot { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
}
