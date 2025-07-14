// Orm.BL/Profiles/OrderProfile.cs
using System.Linq;
using Orm.BL.Dtos.OrderDtos;
using Orm.BL.Dtos.OrderItemDtos;
using Orm.Core.Entities;

namespace Orm.BL.Profiles;

public static class OrderProfile
{
    public static Order OrderCreateToOrder(OrderCreateDto dto)
    {
        return new Order
        {
            OrderDate = dto.OrderDate,
            TableId = dto.TableId, // Mapping TableId
            OrderItems = dto.OrderItems.Select(OrderItemProfile.OrderItemCreateToOrderItem).ToList()
        };
    }

    public static OrderReturnDto OrderToOrderReturnDto(Order entity)
    {
        return new OrderReturnDto
        {
            Id = entity.Id,
            OrderDate = entity.OrderDate,
            TotalOrderPrice = entity.OrderItems.Sum(oi => oi.TotalAmount),
            TotalOrderCount = entity.OrderItems.Sum(oi => oi.Quantity),
            TableId = entity.TableId, // Mapping TableId
            TableNo = entity.Table?.No, // Mapping TableNo (if Table is included)
            OrderItems = entity.OrderItems.Select(OrderItemProfile.OrderItemToOrderItemReturnDto).ToList()
        };
    }
}