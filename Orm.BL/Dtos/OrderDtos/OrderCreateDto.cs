// Orm.BL/Dtos/OrderDtos/OrderCreateDto.cs
using Orm.BL.Dtos.OrderItemDtos;
using System.Collections.Generic;

namespace Orm.BL.Dtos.OrderDtos;

public class OrderCreateDto
{
    public DateTime OrderDate { get; set; }
    public int? TableId { get; set; } // Yeni eklenen alan
    public List<OrderItemCreateDto> OrderItems { get; set; } = new List<OrderItemCreateDto>();
}