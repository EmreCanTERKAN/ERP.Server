using ERP.Server.Domain.Customers;
using ERP.Server.Domain.Enums;
using ERP.Server.Domain.OrderDetails;
using ERPServer.Domain.Abstractions;

public sealed class Order : Entity
{
    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public int OrderNumberYear { get; set; }
    public int OrderNumber { get; set; }
    public DateOnly Date { get; set; }
    public DateOnly DeliveryDate { get; set; }
    public OrderStatusEnum Status { get; set; } = OrderStatusEnum.Pending;
    public List<OrderDetail>? Details { get; set; }

    // Number yerine bir backing field kullanarak değeri cache'leyelim
    private string? _number;
    public string Number => _number ??= GenerateNumber();

    private string GenerateNumber()
    {
        string prefix = "BLG";
        string initialString = prefix + OrderNumberYear.ToString() + OrderNumber.ToString();
        int targetLength = 16;
        int missingLength = targetLength - initialString.Length;
        return prefix + OrderNumberYear.ToString() + new string('0', missingLength) + OrderNumber.ToString();
    }
}
