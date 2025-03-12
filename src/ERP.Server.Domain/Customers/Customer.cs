
using ERPServer.Domain.Abstractions;

namespace ERP.Server.Domain.Customers;

public sealed class Customer : Entity
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName => string.Join(" ", FirstName, LastName);
    public string TaxDepartment { get; set; } = default!;
    public string TaxNumber { get; set; } = default!;
    public Address Address { get; set; } = default!;

}

