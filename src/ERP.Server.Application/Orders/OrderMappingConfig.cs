using ERP.Server.Domain.OrderDetails;
using ERP.Server.Domain.Orders;
using Mapster;

namespace ERP.Server.Application.Orders;

public sealed class OrderMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateOrderCommand, Order>()
                .Map(dest => dest.OrderNumber, src => 0) // OrderNumber daha sonra atanacak
                .Map(dest => dest.OrderNumberYear, src => src.Date.Year)
                .Map(dest => dest.Details, src => src.Details.Adapt<List<OrderDetail>>());

        config.NewConfig<OrderDetailDto, OrderDetail>();
    }
}
