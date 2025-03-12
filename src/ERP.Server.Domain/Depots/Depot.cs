using ERP.Server.Domain.Customers;
using ERPServer.Domain.Abstractions;

namespace ERP.Server.Domain.Depots;

public sealed class Depot : Entity
{
    public string Name { get; set; } = default!;
    public Address Address { get; set; } = default!;
}
