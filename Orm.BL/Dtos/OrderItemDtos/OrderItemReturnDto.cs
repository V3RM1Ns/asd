// Orm.BL/Dtos/OrderItemDtos/OrderItemReturnDto.cs
namespace Orm.BL.Dtos.OrderItemDtos;

public class OrderItemReturnDto // Correct class name for an individual order item
{
    public int Id { get; set; } // The ID of the OrderItem itself
    public int MenuItemId { get; set; }
    public string? MenuItemName { get; set; }
    public int Quantity { get; set; }
    public decimal TotalAmount { get; set; } // Quantity * MenuItem.Price
}