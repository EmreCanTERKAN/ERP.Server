namespace ERP.Server.Domain.Customers;

public sealed record Address
{
    public string? City { get; set; }
    public string? Town { get; set; }
    public string? FullAddress { get; set; }
}

