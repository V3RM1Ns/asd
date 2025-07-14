using Orm.Core.Entities.Common;

namespace Orm.Core.Entities;


public class OrderItem:BaseEntity
{
    public int OrderId { get; set; }
    public Order Orders { get; set; }
    
    public int MenuItemId { get; set; }
    public MenuItem MenuItem { get; set; }
    
    public int TotalAmount { get; set; }
    public int Quantity { get; set; }
}