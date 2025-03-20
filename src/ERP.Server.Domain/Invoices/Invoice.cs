using ERP.Server.Domain.Customers;
using ERP.Server.Domain.Enums;
using ERP.Server.Domain.InvoiceDetails;
using ERPServer.Domain.Abstractions;

public sealed class Invoice : Entity
{
    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public InvoiceTypeEnum Type { get; set; } = InvoiceTypeEnum.Purchase;
    public List<InvoiceDetail>? Details { get; set; }
}
