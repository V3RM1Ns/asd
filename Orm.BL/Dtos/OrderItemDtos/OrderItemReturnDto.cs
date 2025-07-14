// Orm.BL/Dtos/OrderDtos/OrderReturnDto.cs
using Orm.BL.Dtos.OrderItemDtos;
using System;
using System.Collections.Generic;

namespace Orm.BL.Dtos.OrderDtos;

public class OrderItemReturnDto
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalOrderPrice { get; set; }
    public int TotalOrderCount { get; set; }
    public int? TableId { get; set; } // Yeni eklenen alan
    public string? TableNo { get; set; } // Yeni eklenen alan

    public List<OrderItemReturnDto> OrderItems { get; set; } = new List<OrderItemReturnDto>();
}