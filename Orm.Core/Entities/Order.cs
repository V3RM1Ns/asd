// Orm.Core/Entities/Order.cs
using Orm.Core.Entities.Common;
using System;
using System.Collections.Generic;

namespace Orm.Core.Entities;

public class Order : BaseEntity
{
    public DateTime OrderDate { get; set; }
    public decimal TotalOrderPrice { get; set; }
    public int Count { get; set; } // Total count of items in the order
    public int? TableId { get; set; } // Foreign key for Table - nullable means order can be take away
    public Table? Table { get; set; } // Navigation property

    public ICollection<OrderItem> OrderItems { get; set; }

    public Order()
    {
        OrderItems = new HashSet<OrderItem>();
    }
}