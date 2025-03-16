﻿using ERP.Server.Domain.Customers;
using ERP.Server.Domain.Enums;
using ERP.Server.Domain.OrderDetails;
using ERPServer.Domain.Abstractions;

namespace ERP.Server.Domain.Orders;

public sealed class Order : Entity
{
    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public DateTime DeliveryDate { get; set; }
    public OrderStatusEnum Status { get; set; } = OrderStatusEnum.Pending;
    public List<OrderDetail>? Details { get; set; }
}
