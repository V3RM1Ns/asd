using Orm.Core.Entities.Common;

namespace Orm.Core.Entities;

public class MenuItem:BaseEntity
{ 
    public string Name { get; set; }
    public int Price { get; set; }
    public Category Category { get; set; }
    public OrderItem OrderItem { get; set; }
}