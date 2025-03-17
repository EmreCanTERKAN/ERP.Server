using ERP.Server.Domain.Enums;
using ERP.Server.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Server.Infrastructure.Configurations;

internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(p => p.Status)
            .HasConversion(status => status.Value, value => OrderStatusEnum.FromValue(value));
    }
}
